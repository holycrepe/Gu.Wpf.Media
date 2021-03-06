﻿namespace Gu.Wpf.Media.Tests
{
    using System;
    using System.Globalization;

    using NUnit.Framework;

    public class TimeSpanToStringConverterTests
    {
        [TestCase("00:00:01", "0:01")]
        [TestCase("00:00:12", "0:12")]
        [TestCase("00:01:23", "1:23")]
        [TestCase("00:12:34", "12:34")]
        [TestCase("12:34:56", "12:34:56")]
        public void Roundtrip(string timeString, string expected)
        {
            var time = TimeSpan.ParseExact(timeString, @"hh\:mm\:ss", CultureInfo.InvariantCulture);
            var converted = (string)TimeSpanToStringConverter.Default.Convert(time, null, null, null);
            Assert.AreEqual(expected, converted);
            var roundtrip = (TimeSpan)TimeSpanToStringConverter.Default.ConvertBack(converted, null, null, null);
            Assert.AreEqual(time, roundtrip);
        }

        [TestCase("00:00:12.0", "FFF", "0:12")]
        [TestCase("00:00:12.0", "fff", "0:12.000")]
        [TestCase("00:00:12.12345", "FFF", "0:12.123")]
        [TestCase("00:00:12.12345", "fff", "0:12.123")]
        public void RoundtripWithParameter(string timeString, string parameter, string expected)
        {
            var time = TimeSpan.ParseExact(timeString, @"hh\:mm\:ss\.FFFFFFF", CultureInfo.InvariantCulture);
            var converted = (string)TimeSpanToStringConverter.Default.Convert(time, null, parameter, null);
            Assert.AreEqual(expected, converted);
            var roundtrip = (TimeSpan)TimeSpanToStringConverter.Default.ConvertBack(converted, null, null, null);
            Assert.AreEqual(expected, (string)TimeSpanToStringConverter.Default.Convert(roundtrip, null, parameter, null));
        }

        [Test]
        public void RoundtripNull()
        {
            var converted = (string)TimeSpanToStringConverter.Default.Convert(null, null, null, null);
            Assert.AreEqual("-:--", converted);
            var roundtrip = TimeSpanToStringConverter.Default.ConvertBack(converted, null, null, null);
            Assert.AreEqual(null, roundtrip);
        }
    }
}
