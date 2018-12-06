using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Rowa.Lib.Log
{
    /// <summary>
    /// This Class Reads and Manipulates Template Strings of the Logger Configuration
    /// </summary>
    internal class LogConfigTemplate
    {
        #region Members
        /// <summary>
        /// Holds the Key/Value Pairs of the Template
        /// </summary>
        private readonly Dictionary<string, LogConfigTemplateValue> _templateValues = new Dictionary<string, LogConfigTemplateValue>();

        /// <summary>
        /// Holds the actual Template with all Values
        /// </summary>
        private readonly List<LogConfigTemplateValue> _template = new List<LogConfigTemplateValue>();

        /// <summary>
        /// The Original String from which this Template was made
        /// </summary>
        public string TemplateString { get; private set; }
        #endregion

        #region non-public Methods
        /// <summary>
        /// Loads a Template from a String
        /// </summary>
        /// <param name="templateString">string with Template in it</param>
        private void LoadFromTemplateString(string templateString)
        {
            TemplateString = templateString;
            var regex = new Regex(@"\{(?<paramvalue>.*?)\}");

            var stringpos = 0;

            foreach (Match match in regex.Matches(templateString))
            {
                if (match.Index > stringpos)
                {
                    _template.Add(new LogConfigTemplateValue
                    {
                        Type = LogConfigTemplateValueType.Constant,
                        Value = templateString.Substring(stringpos, match.Index - stringpos)
                    });
                }


                var value = GetTemplateValue(match.Groups["paramvalue"].Value);

                if (!_templateValues.ContainsKey(value.Name))
                {
                    _templateValues.Add(value.Name, value);
                }

                _template.Add(_templateValues[value.Name]);

                stringpos = match.Index + match.Length;
            }


            if (stringpos < templateString.Length)
            {
                _template.Add(new LogConfigTemplateValue
                {
                    Type = LogConfigTemplateValueType.Constant,
                    Value = templateString.Substring(stringpos, templateString.Length - stringpos)
                });
            }
        }

        /// <summary>
        /// Converts a String to a TemplateValue
        /// </summary>
        /// <param name="value">string to be converted</param>
        /// <returns>resulting Template Value</returns>
        private LogConfigTemplateValue GetTemplateValue(string value)
        {
            var regex = new Regex(@"(?<paramname>.+)\[(?<paramformat>.*)\]");

            if (regex.IsMatch(value))
            {
                return new LogConfigTemplateValue
                {
                    Type = LogConfigTemplateValueType.Value,
                    Name = regex.Match(value).Groups["paramname"].Value,
                    Format = regex.Match(value).Groups["paramformat"].Value
                };
            }
            else
            {
                return new LogConfigTemplateValue
                {
                    Type = LogConfigTemplateValueType.Value,
                    Name = value
                };
            }
        }
        #endregion


        #region public Methods
        /// <summary>
        /// Initializes the Template from a String
        /// </summary>
        /// <param name="templateString">Source Template String</param>
        public LogConfigTemplate(string templateString)
        {
            LoadFromTemplateString(templateString);
        }

        /// <summary>
        /// Initializes the Template from another Template
        /// </summary>
        /// <param name="template">Source Template</param>
        public LogConfigTemplate(LogConfigTemplate template)
        {
            LoadFromTemplateString(template.TemplateString);

            foreach (var templateValue in template._templateValues)
            {
                _templateValues[templateValue.Key].Value = templateValue.Value.Value;
            }
        }

        /// <summary>
        /// Sets a Template variable to the desired Value
        /// </summary>
        /// <param name="valueName">The Key to the Value</param>
        /// <param name="value">The New Value</param>
        /// <returns>returns itself</returns>
        public LogConfigTemplate SetValue(string valueName, string value)
        {
            if (!_templateValues.ContainsKey(valueName)) return this;
            if (_templateValues[valueName].Type != LogConfigTemplateValueType.Value) return this;

            _templateValues[valueName].Value = string.IsNullOrEmpty(_templateValues[valueName].Format) ? value : string.Format(_templateValues[valueName].Format, value);

            return this;
        }

        /// <summary>
        /// Sets a Template variable to the desired Value
        /// </summary>
        /// <param name="valueName">The Key to the Value</param>
        /// <param name="value">The New Value</param>
        /// <returns>returns itself</returns>
        public LogConfigTemplate SetValue(string valueName, DateTime value)
        {
            if (!_templateValues.ContainsKey(valueName)) return this;
            if (_templateValues[valueName].Type != LogConfigTemplateValueType.Value) return this;

            _templateValues[valueName].Value = value.ToString(_templateValues[valueName].Format);

            return this;
        }

        /// <summary>
        /// Sets a Template variable to the desired Value
        /// </summary>
        /// <param name="valueName">The Key to the Value</param>
        /// <param name="value">The New Value</param>
        /// <returns>returns itself</returns>
        public LogConfigTemplate SetValue(string valueName, long value)
        {
            if (!_templateValues.ContainsKey(valueName)) return this;
            if (_templateValues[valueName].Type != LogConfigTemplateValueType.Value) return this;

            _templateValues[valueName].Value = string.IsNullOrEmpty(_templateValues[valueName].Format) ? value.ToString() : value.ToString(_templateValues[valueName].Format);

            return this;
        }

        /// <summary>
        /// Returns the generated Template as string
        /// </summary>
        /// <returns>the template as string</returns>
        public override string ToString()
        {
            return string.Join<LogConfigTemplateValue>("", _template.ToArray());
        }
        #endregion
    }
}
