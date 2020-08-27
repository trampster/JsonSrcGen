
using System;
using System.Text;

namespace JsonSG
{
    public class JsonSGConvert
    {
        [ThreadStatic]
        StringBuilder Builder;
        public string ToJson(UnitTests.CollisionJsonClass value)
        {

            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();
            builder.Append("{\"Aaa\":\"");
            builder.Append(value.Aaa);
            builder.Append("\",\"Aab\":\"");
            builder.Append(value.Aab);
            builder.Append("\",\"Abb\":\"");
            builder.Append(value.Abb);
            builder.Append("\"}");
            return builder.ToString();
        }
        public void FromJson(UnitTests.CollisionJsonClass value, string jsonString)
        {
            var json = jsonString.AsSpan();
            json = json.SkipWhitespaceTo('{');
            while(true)
            {
                json = json.SkipWhitespaceTo('\"');
                var propertyName = json.ReadTo('\"');
                json = json.Slice(propertyName.Length + 1);
                json = json.SkipWhitespaceTo(':');
                switch(propertyName[2 % propertyName.Length] % 3)
                {
                    case 1:
                        if(!propertyName.EqualsString("Aaa"))
                        {
                            break;
                        }
                        json = json.ReadString(out string propertyAaaValue);
                        value.Aaa = propertyAaaValue;
                        break;
                    case 2:
                        switch(propertyName[1 % propertyName.Length] % 2)
                        {
                            case 0:
                                if(!propertyName.EqualsString("Abb"))
                                {
                                    break;
                                }
                                json = json.ReadString(out string propertyAbbValue);
                                value.Abb = propertyAbbValue;
                                break;
                            case 1:
                                if(!propertyName.EqualsString("Aab"))
                                {
                                    break;
                                }
                                json = json.ReadString(out string propertyAabValue);
                                value.Aab = propertyAabValue;
                                break;
                        }
                        break;
                }
                json = json.SkipWhitespaceTo(',', '}', out char found);
                if(found == '}')
                {
                    return;
                }
            }
        }
        public string ToJson(UnitTests.JsonIntClass value)
        {

            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();
            builder.Append("{\"Age\":");
            builder.Append(value.Age);
            builder.Append(",\"Height\":");
            builder.Append(value.Height);
            builder.Append(",\"Max\":");
            builder.Append(value.Max);
            builder.Append(",\"Min\":");
            builder.Append(value.Min);
            builder.Append(",\"Zero\":");
            builder.Append(value.Zero);
            builder.Append("}");
            return builder.ToString();
        }
        public void FromJson(UnitTests.JsonIntClass value, string jsonString)
        {
            var json = jsonString.AsSpan();
            json = json.SkipWhitespaceTo('{');
            while(true)
            {
                json = json.SkipWhitespaceTo('\"');
                var propertyName = json.ReadTo('\"');
                json = json.Slice(propertyName.Length + 1);
                json = json.SkipWhitespaceTo(':');
                switch(propertyName[2 % propertyName.Length] % 7)
                {
                    case 0:
                        if(!propertyName.EqualsString("Height"))
                        {
                            break;
                        }
                        json = json.ReadInt(out int propertyHeightValue);
                        value.Height = propertyHeightValue;
                        break;
                    case 1:
                        if(!propertyName.EqualsString("Max"))
                        {
                            break;
                        }
                        json = json.ReadInt(out int propertyMaxValue);
                        value.Max = propertyMaxValue;
                        break;
                    case 2:
                        if(!propertyName.EqualsString("Zero"))
                        {
                            break;
                        }
                        json = json.ReadInt(out int propertyZeroValue);
                        value.Zero = propertyZeroValue;
                        break;
                    case 3:
                        if(!propertyName.EqualsString("Age"))
                        {
                            break;
                        }
                        json = json.ReadInt(out int propertyAgeValue);
                        value.Age = propertyAgeValue;
                        break;
                    case 5:
                        if(!propertyName.EqualsString("Min"))
                        {
                            break;
                        }
                        json = json.ReadInt(out int propertyMinValue);
                        value.Min = propertyMinValue;
                        break;
                }
                json = json.SkipWhitespaceTo(',', '}', out char found);
                if(found == '}')
                {
                    return;
                }
            }
        }
        public string ToJson(UnitTests.JsonClass value)
        {

            var builder = Builder;
            if(builder == null)
            {
                builder = new StringBuilder();
                Builder = builder;
            }
            builder.Clear();
            builder.Append("{\"FirstName\":\"");
            builder.Append(value.FirstName);
            builder.Append("\",\"LastName\":\"");
            builder.Append(value.LastName);
            builder.Append("\"}");
            return builder.ToString();
        }
        public void FromJson(UnitTests.JsonClass value, string jsonString)
        {
            var json = jsonString.AsSpan();
            json = json.SkipWhitespaceTo('{');
            while(true)
            {
                json = json.SkipWhitespaceTo('\"');
                var propertyName = json.ReadTo('\"');
                json = json.Slice(propertyName.Length + 1);
                json = json.SkipWhitespaceTo(':');
                switch(propertyName.Length % 2)
                {
                    case 0:
                        if(!propertyName.EqualsString("LastName"))
                        {
                            break;
                        }
                        json = json.ReadString(out string propertyLastNameValue);
                        value.LastName = propertyLastNameValue;
                        break;
                    case 1:
                        if(!propertyName.EqualsString("FirstName"))
                        {
                            break;
                        }
                        json = json.ReadString(out string propertyFirstNameValue);
                        value.FirstName = propertyFirstNameValue;
                        break;
                }
                json = json.SkipWhitespaceTo(',', '}', out char found);
                if(found == '}')
                {
                    return;
                }
            }
        }
    }
}
