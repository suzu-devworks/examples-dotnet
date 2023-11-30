namespace Examples.Serialization.Xml.Tests;

public partial class QueryXDocumentVsXElementTests
{
    // <see href="https://www.drk7.jp/weather/xml/01.xml" />

    private static readonly string ResponseXml = """
        <?xml version="1.0" encoding="UTF-8"?>
        <weatherforecast>
            <title>weather forecast xml</title>
            <link>http://www.drk7.jp/weather/xml/01.xml</link>
            <description>気象庁の天気予報情報を XML で配信。1日1回 AM 6:00 ごろ更新。</description>
            <pubDate>Wed, 9 Nov 2022 06:00:02 +0900</pubDate>
            <author>気象庁</author>
            <managingEditor>drk7.jp</managingEditor>
            <pref id="北海道">
                <area id="上川地方">
                    <info date="2022/11/10">
                        <weather>曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/201.gif</img>
                        <weather_detail>くもり　昼前　から　夕方　晴れ　所により　明け方　まで　雨　で　雷を伴う</weather_detail>
                        <wave></wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">11</range>
                        <range centigrade="min">6</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">20</period>
                        <period hour="06-12">10</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">10</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="北見地方">
                    <info date="2022/11/10">
                        <weather>曇時々晴</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/201.gif</img>
                        <weather_detail>くもり　時々　晴れ</weather_detail>
                        <wave></wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">13</range>
                        <range centigrade="min">3</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">0</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="十勝地方">
                    <info date="2022/11/10">
                        <weather>晴時々曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/101.gif</img>
                        <weather_detail>晴れ　昼前　まで　時々　くもり</weather_detail>
                        <wave>２メートル　後　１．５メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">16</range>
                        <range centigrade="min">2</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">0</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="宗谷地方">
                    <info date="2022/11/10">
                        <weather>曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/210.gif</img>
                        <weather_detail>くもり　昼前　から　時々　晴れ　所により　明け方　まで　雨　で　雷を伴う</weather_detail>
                        <wave>３メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">8</range>
                        <range centigrade="min">7</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">20</period>
                        <period hour="06-12">10</period>
                        <period hour="12-18">10</period>
                        <period hour="18-24">10</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="後志地方">
                    <info date="2022/11/10">
                        <weather>曇時々晴</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/210.gif</img>
                        <weather_detail>くもり　昼前　から　時々　晴れ</weather_detail>
                        <wave>３メートル　後　２メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">11</range>
                        <range centigrade="min">7</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">10</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="日高地方">
                    <info date="2022/11/10">
                        <weather>晴時々曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/101.gif</img>
                        <weather_detail>晴れ　昼過ぎ　まで　時々　くもり</weather_detail>
                        <wave>２メートル　後　１．５メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">14</range>
                        <range centigrade="min">10</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">10</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="根室地方">
                    <info date="2022/11/10">
                        <weather>晴時々曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/101.gif</img>
                        <weather_detail>晴れ　昼過ぎ　まで　時々　くもり</weather_detail>
                        <wave>２メートル　後　３メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">15</range>
                        <range centigrade="min">8</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">0</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="檜山地方">
                    <info date="2022/11/10">
                        <weather>晴時々曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/201.gif</img>
                        <weather_detail>くもり　時々　晴れ</weather_detail>
                        <wave>２．５メートル　後　２メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">15</range>
                        <range centigrade="min">12</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">10</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="渡島地方">
                    <info date="2022/11/10">
                        <weather>晴時々曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/101.gif</img>
                        <weather_detail>晴れ　昼過ぎ　まで　時々　くもり</weather_detail>
                        <wave>２．５メートル　後　２メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">15</range>
                        <range centigrade="min">8</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">10</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
            </area>
                <area id="留萌地方">
                    <info date="2022/11/10">
                        <weather>曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/201.gif</img>
                        <weather_detail>くもり　昼前　から　夕方　晴れ　所により　明け方　まで　雨　で　雷を伴う</weather_detail>
                        <wave>３メートル　後　２メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">12</range>
                        <range centigrade="min">9</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">20</period>
                        <period hour="06-12">10</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">10</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="石狩地方">
                    <info date="2022/11/10">
                        <weather>曇時々晴</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/210.gif</img>
                        <weather_detail>くもり　昼前　から　時々　晴れ　所により　明け方　まで　雨　で　雷を伴う</weather_detail>
                        <wave>３メートル　後　２メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">13</range>
                        <range centigrade="min">8</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">20</period>
                        <period hour="06-12">10</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="空知地方">
                    <info date="2022/11/10">
                        <weather>曇時々晴</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/210.gif</img>
                        <weather_detail>くもり　昼前　から　時々　晴れ　所により　明け方　まで　雨　で　雷を伴う</weather_detail>
                        <wave></wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">12</range>
                        <range centigrade="min">8</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">20</period>
                        <period hour="06-12">10</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="紋別地方">
                    <info date="2022/11/10">
                        <weather>曇時々晴</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/210.gif</img>
                        <weather_detail>くもり　昼前　から　時々　晴れ</weather_detail>
                        <wave>１．５メートル　後　３メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">11</range>
                        <range centigrade="min">7</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">10</period>
                        <period hour="06-12">10</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="網走地方">
                    <info date="2022/11/10">
                        <weather>曇時々晴</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/201.gif</img>
                        <weather_detail>くもり　時々　晴れ</weather_detail>
                        <wave>１．５メートル　後　３メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">12</range>
                        <range centigrade="min">6</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">0</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="胆振地方">
                    <info date="2022/11/10">
                        <weather>晴時々曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/101.gif</img>
                        <weather_detail>晴れ　昼過ぎ　まで　時々　くもり</weather_detail>
                        <wave>１．５メートル　後　１メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">14</range>
                        <range centigrade="min">10</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">10</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
                <area id="釧路地方">
                    <info date="2022/11/10">
                        <weather>晴時々曇</weather>
                        <img>http://www.drk7.jp/MT/images/MTWeather/101.gif</img>
                        <weather_detail>晴れ　昼前　まで　時々　くもり</weather_detail>
                        <wave>２メートル　後　１．５メートル</wave>
                        <temperature unit="摂氏">
                        <range centigrade="max">15</range>
                        <range centigrade="min">7</range>
                        </temperature>
                        <rainfallchance unit="％">
                        <period hour="00-06">0</period>
                        <period hour="06-12">0</period>
                        <period hour="12-18">0</period>
                        <period hour="18-24">0</period>
                        </rainfallchance>
                    </info>
                </area>
            </pref>
        </weatherforecast>
        """;

}
