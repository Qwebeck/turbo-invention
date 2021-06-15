using PythonInterpreter.Visitors;
using System;
using System.Collections.Generic;
using System.Text;
using PythonInterpreter.Extensions;
using Xunit;
using FluentAssertions;
using PythonInterpreter.Tests.Helpers;

namespace PythonInterpreter.Tests
{
    public class EnumerableTests
    {
        private readonly MainVisitor mainVisitor = new MainVisitor();


        [Fact]
        public void RangeShouldReturnEnumerable()
        {
            // Arrange
            //var testString = "1 .. 5";
            //// Act
            //var result = 
            // Assert

        }

        //[Fact]
        //public void ShouldIterateAndPrintValuesFromRange()
        //{
        //    // Arrange
        //    var testString = @"
        //    for i in 1 .. 5:
        //        print(i)
        //    end
        //    ";
        //    var expectedOutput = string.Join(Environment.NewLine, new int[]
        //    {
        //        1, 2, 3, 4
        //    });
        //    using var mockConsole = new ConsoleMock();
        //    // Act
        //    mainVisitor.RunOnInput(testString);
        //    // Assert
        //    mockConsole.Content.Should().Contain(expectedOutput);
        //}


        //[Fact]
        //public void ShouldIterateAndPrintValuesFromList()
        //{
        //    // Arrange
        //    var testString = @"
        //    for i in [1, 2, 3, 4]:
        //        print(i)
        //    end
        //    ";
        //    var expectedOutput = string.Join(Environment.NewLine, new int[]
        //    {
        //        1, 2, 3, 4
        //    });
        //    using var mockConsole = new ConsoleMock();
        //    // Act
        //    mainVisitor.RunOnInput(testString);
        //    // Assert
        //    mockConsole.Content.Should().Contain(expectedOutput);
        //}

    }
}
