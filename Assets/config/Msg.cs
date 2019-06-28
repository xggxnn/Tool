
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class Msg
{
    /**
     * 数据bufer
     * */
    MemoryStream _buf;

    /**
     * 消息号
     * */
    private int _msgtype;

    /**
     * 消息体长度
     * */
    private int _msglen;



    /**
     * 包头长度。 常量
     * */
    public static int MsgHeaderLength = 4 + 2;


    public Msg()
    {
        _buf = new MemoryStream(1024);
        Reset();
    }

    ~Msg()
    {
    }

    /**
     * socket收到消息时调用。此方法外部不要调用。
     * */
    public void _CallOnRecv(byte[] data, int datalen)
    {
        _buf.Position = 0;
        _msglen = datalen;

        _buf.Write(data, 0, datalen);

        _buf.Position = 4;
        _msgtype = GetInt16();
    }

    /**
     * socket的SendMsg方法内部调用。此方法外部不要调用。
     * */
    public void _CallForSend()
    {
        _msglen = (int)_buf.Position;
        _buf.Position = 0;

        PushInt32(_msglen);
        PushInt16((Int16)_msgtype);
    }

    public byte[] GetRawByteArray()
    {
        return _buf.GetBuffer();
    }

    /**
     * 当从lua层收到消息数据时，组装消息
     * @param {int} msglen 消息长度
     * @param {int} msgtype 消息号
     * @param {byte[]} data 消息体
     * @param {int} datalen 消息体长度
     * */
    public void _CallOnRecvFromLua(int msglen, int msgtype, byte[] data, int datalen)
    {
        _msglen = msglen;
        _msgtype = msgtype;

        _buf.Position = 0;

        PushInt32(msglen);
        PushInt16((Int16)msgtype);
        _buf.Write(data, 0, datalen);

        _buf.Position = MsgHeaderLength;
    }

    /**
     * 获取消息体字节流数据(不包含消息头)
     * */
    public byte[] GetMsgBodyBytes()
    {
        byte[] temp = _buf.GetBuffer();
        byte[] dst = new byte[_msglen - MsgHeaderLength];
        Buffer.BlockCopy(temp, MsgHeaderLength, dst, 0, _msglen - MsgHeaderLength);
        return dst;
    }

    /**
     * 获取消息体长度(不包含消息头)
     * */
    public int GetMsgBodyLength()
    {
        return _msglen - MsgHeaderLength;
    }

    /**
     * 设置包长度
     * */
    public void SetLength(int len)
    {
        _msglen = len;
        _buf.Position = len;
    }

    /**
     * 获取包长度
     * */
    public int GetLength()
    {
        return _msglen;
    }

    /**
     * 设置消息类型
     * */
    public void SetMsgType(int type)
    {
        _msgtype = type;
    }

    /**
     * 获取消息类型
     * */
    public int GetMsgType()
    {
        return _msgtype;
    }

    /**
     * 压入1个字节
     * */
    public void PushSByte(SByte value)
    {
        _buf.WriteByte((byte)value);
    }

    /**
     * 压入1个字节
     * */
    public void PushByte(Byte value)
    {
        _buf.WriteByte(value);
    }



    /**
     * 压入byte[]
     * */
    public void PushBytes(byte[] value)
    {
        _buf.Write(value, 0, value.Length);
    }

    /**
     * 装入布尔类型
     * */
    public void PushBoolean(bool value)
    {
        byte tmp = 1;
        if (!value)
            tmp = 0;
        _buf.WriteByte(tmp);
    }

    /**
     * 压入1个字节
     * */
    public void PushInt8(SByte value)
    {
        _buf.WriteByte((byte)value);
    }

    /**
     * 压入1个字节
     * */
    public void PushUInt8(Byte value)
    {
        _buf.WriteByte((byte)value);
    }

    /**
     * 压入2个字节
     * */
    public void PushInt16(Int16 value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 2);
    }

    /**
     * 压入2个字节
     * */
    public void PushUInt16(UInt16 value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 2);
    }

    /**
     * 压入4个字节
     * */
    public void PushInt32(Int32 value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 4);
    }

    /**
     * 压入4个字节
     * */
    public void PushUInt32(UInt32 value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 4);
    }

    /**
     * 压入8个字节
     * 注意考虑服务器与客户端交互的最大值。
     * */
    public void PushInt64(Int64 value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 8);
    }

    /**
     * 压入8个字节
     * 注意考虑服务器与客户端交互的最大值。
     * */
    public void PushUInt64(UInt64 value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 8);
    }

    /**
     * 压入单精度浮点数(4字节)
     * */
    public void PushFloat(float value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 4);
    }

    /**
     * 压入双精度浮点数(8字节)
     * */
    public void PushDouble(double value)
    {
        _buf.Write(BitConverter.GetBytes(value), 0, 8);
    }

    /**
     * 压入一个字符串。 先压入2字节的字符串长度（无符号，64K最大），然后压入字符串数据，不包含C格式的字符串结尾符。
     * */
    public void PushString(string value)
    {
        byte[] tmp = Encoding.UTF8.GetBytes(value);
        PushUInt16((UInt16)tmp.Length);
        _buf.Write(tmp, 0, tmp.Length);
    }

    /**
     * 压入一个固定长度的字符串；最大固定长度为512字节。
     * */
    public void PushFixedString(string value, int fixedlen)
    {
        if (fixedlen > 512)
        {
            Debug.LogWarning("要写入的固定长度不能大于512字节！");
            return;
        }

        byte[] tmp = Encoding.UTF8.GetBytes(value);
        int len = tmp.Length;
        if (len > fixedlen)
            len = fixedlen;

        int secondpush = fixedlen - len;

        PushInt16((Int16)fixedlen);
        if (len > 0)
            _buf.Write(tmp, 0, fixedlen - secondpush);

        while (secondpush > 0)
        {
            PushByte(0);
            secondpush--;
        }
    }

    /**
     * 当收到消息后，从消息中获取数据时，记得先调用此方法。
     * */
    public void Begin()
    {
        _buf.Position = MsgHeaderLength;
    }

    /**
     * 此方法在于重复利用此对象发送多个消息。
     * */
    public void Reset()
    {
        _buf.Position = MsgHeaderLength;
        _msgtype = -1;
        _msglen = 0;
    }

    /**
     * 获取1个字节
     * */
    public SByte GetSByte()
    {
        if (_buf.Position + 1 > _msglen)
            return 0;

        int temp = _buf.ReadByte();
        return (SByte)temp;
    }

    /**
     * 获取1个字节
     * */
    public Byte GetByte()
    {
        if (_buf.Position + 1 > _msglen)
            return 0;

        int temp = _buf.ReadByte();
        return (Byte)temp;
    }

    /**
     * 获取布尔类型
     * */
    public bool GetBoolean()
    {
        if (_buf.Position + 1 > _msglen)
            return false;

        int tmp = _buf.ReadByte();
        if (tmp == 1)
            return true;
        else
            return false;
    }

    /**
     * 获取1个字节
     * */
    public SByte GetInt8()
    {
        if (_buf.Position + 1 > _msglen)
            return 0;

        int temp = _buf.ReadByte();
        return (SByte)temp;
    }

    /**
     * 获取1个字节
     * */
    public Byte GetUInt8()
    {
        if (_buf.Position + 1 > _msglen)
            return 0;

        int temp = _buf.ReadByte();
        return (Byte)temp;
    }

    /**
     * 获取2个字节
     * */
    public Int16 GetInt16()
    {
        if (_buf.Position + 2 > _msglen)
            return 0;

        byte[] tmp = new byte[2];
        _buf.Read(tmp, 0, 2);

        return BitConverter.ToInt16(tmp, 0);
    }

    /**
     * 获取2个字节
     * */
    public UInt16 GetUInt16()
    {
        if (_buf.Position + 2 > _msglen)
            return 0;

        byte[] tmp = new byte[2];
        _buf.Read(tmp, 0, 2);
        return BitConverter.ToUInt16(tmp, 0);
    }

    /**
     * 获取4个字节
     * */
    public Int32 GetInt32()
    {
        if (_buf.Position + 4 > _msglen)
            return 0;

        byte[] tmp = new byte[4];
        _buf.Read(tmp, 0, 4);
        return BitConverter.ToInt32(tmp, 0);
    }

    /**
     * 获取4个字节
     * */
    public UInt32 GetUInt32()
    {
        if (_buf.Position + 4 > _msglen)
            return 0;

        byte[] tmp = new byte[4];
        _buf.Read(tmp, 0, 4);
        return BitConverter.ToUInt32(tmp, 0);
    }

    /**
     * 获取8个字节
     * 注意考虑服务器与客户端交互的最大值。
     * */
    public Int64 GetInt64()
    {
        if (_buf.Position + 8 > _msglen)
            return 0;

        byte[] tmp = new byte[8];
        _buf.Read(tmp, 0, 8);

        return BitConverter.ToInt64(tmp, 0);
    }

    /**
     * 获取8个字节
     * 注意考虑服务器与客户端交互的最大值。
     * */
    public UInt64 GetUInt64()
    {
        if (_buf.Position + 8 > _msglen)
            return 0;

        byte[] tmp = new byte[8];
        _buf.Read(tmp, 0, 8);

        return BitConverter.ToUInt64(tmp, 0);
    }

    /**
     * 获取单精度浮点数(4字节)
     * */
    public float GetFloat()
    {
        if (_buf.Position + 4 > _msglen)
            return 0;

        byte[] tmp = new byte[4];
        _buf.Read(tmp, 0, 4);
        return BitConverter.ToSingle(tmp, 0);
    }

    /**
     * 获取双精度浮点数(8字节)
     * */
    public double GetDouble()
    {
        if (_buf.Position + 8 > _msglen)
            return 0;

        byte[] tmp = new byte[8];
        _buf.Read(tmp, 0, 8);
        return BitConverter.ToDouble(tmp, 0);
    }

    /**
     * 获取一个字符串。先获取2字节的字符串长度信息（无符号，64K最大），然后获取字符串信息。
     * */
    public string GetString()
    {
        if (_buf.Position + 2 > _msglen)
            return String.Empty;

        int len = GetUInt16();
        if (len <= 0)
            return String.Empty;

        if (_buf.Position + len > _msglen)
            return string.Empty;

        byte[] tmp = new byte[len];
        _buf.Read(tmp, 0, len);

        return Encoding.UTF8.GetString(tmp);
    }
}