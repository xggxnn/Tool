﻿public interface IParseCsv
{
    void ParseHeadKeyCN(string[] csv);
    void ParseHeadKeyEN(string[] csv);
    void ParseHeadPropId(string[] csv);
    void ParseCsv(string[] csv);
}
