﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;
using SimcProfileParser.Interfaces;
using SimcProfileParser.Model.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimcProfileParser.Tests
{
    [TestFixture]
    class SimcParserService_ParseProfileTests
    {
        SimcParsedProfile ParsedProfile { get; set; }

        [OneTimeSetUp]
        public async Task InitOnce()
        {
            // Configure Logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.File("logs\\SimcProfileParser.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Load a data file
            var testFile = @"RawData\Hierophant.simc";
            var testFileContents = await File.ReadAllLinesAsync(testFile);
            var testFileString = new List<string>(testFileContents);

            // Create a new profile service
            using var loggerFactory = LoggerFactory.Create(builder => builder
                .AddSerilog()
                .AddFilter(level => level >= LogLevel.Trace));
            var logger = loggerFactory.CreateLogger<SimcParserService>();
            var simcParser = new SimcParserService(logger);

            ParsedProfile = simcParser.ParseProfileAsync(testFileString);
        }


        [Test]
        public void SPS_Parses_Version()
        {
            // Arrange
            var version = "9.0.1-alpha-10";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.SimcAddonVersion);
            Assert.AreEqual(version, ParsedProfile.SimcAddonVersion);
        }

        [Test]
        public void SPS_Parses_Collection_Date()
        {
            // Arrange
            DateTime.TryParse("2020-09-27 01:41", out DateTime parsedDateTime);

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.CollectionDate);
            Assert.AreEqual(parsedDateTime, ParsedProfile.CollectionDate);
        }

        [Test]
        public void SPS_Parses_Character_Name()
        {
            // Arrange
            var charName = "Hierophant";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Name);
            Assert.AreEqual(charName, ParsedProfile.Name);
        }

        [Test]
        public void SPS_Parses_Level()
        {
            // Arrange
            var level = 60;

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Level);
            Assert.AreEqual(level, ParsedProfile.Level);
        }

        [Test]
        public void SPS_Parses_Race()
        {
            // Arrange
            var race = "undead";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Race);
            Assert.AreEqual(race, ParsedProfile.Race);
        }

        [Test]
        public void SPS_Parses_Region()
        {
            // Arrange
            var region = "us";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Region);
            Assert.AreEqual(region, ParsedProfile.Region);
        }

        [Test]
        public void SPS_Parses_Server()
        {
            // Arrange
            var server = "torghast";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Server);
            Assert.AreEqual(server, ParsedProfile.Server);
        }

        [Test]
        public void SPS_Parses_Role()
        {
            // Arrange
            var role = "attack";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Role);
            Assert.AreEqual(role, ParsedProfile.Role);
        }

        [Test]
        public void SPS_Parses_Spec()
        {
            // Arrange
            var spec = "holy";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Spec);
            Assert.AreEqual(spec, ParsedProfile.Spec);
        }

        [Test]
        public void SPS_Parses_Renown()
        {
            // Arrange
            var renown = 40;

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Renown);
            Assert.AreEqual(renown, ParsedProfile.Renown);
        }

        [Test]
        public void SPS_Parses_Covenant()
        {
            // Arrange
            var covenant = "night_fae";

            // Act

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Covenant);
            Assert.AreEqual(covenant, ParsedProfile.Covenant);
        }

        [Test]
        public void SPS_Parses_Conduits()
        {
            // Arrange
            // 116:1/78:1/82:1/84:1/101:1/69:1/73:1/67:1/66:1
            var allConduits = new List<SimcParsedConduit>()
            {
                new SimcParsedConduit() { ConduitId = 116, Rank = 1 },
                new SimcParsedConduit() { ConduitId = 78, Rank = 7 },
                new SimcParsedConduit() { ConduitId = 82, Rank = 1 },
                new SimcParsedConduit() { ConduitId = 84, Rank = 1 },
                new SimcParsedConduit() { ConduitId = 101, Rank = 1 },
                new SimcParsedConduit() { ConduitId = 69, Rank = 1 },
                new SimcParsedConduit() { ConduitId = 73, Rank = 1 },
                new SimcParsedConduit() { ConduitId = 67, Rank = 10 },
                new SimcParsedConduit() { ConduitId = 66, Rank = 1 },
            };

            // Act
            var expectedConduits = JsonConvert.SerializeObject(allConduits);
            var actualConduits = JsonConvert.SerializeObject(ParsedProfile.Conduits);

            // Assert
            Assert.IsNotNull(ParsedProfile);
            Assert.IsNotNull(ParsedProfile.Conduits);
            Assert.NotZero(ParsedProfile.Conduits.Count);
            Assert.AreEqual(expectedConduits, actualConduits);
        }
    }
}