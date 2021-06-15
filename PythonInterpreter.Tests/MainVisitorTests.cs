using System;
using Xunit;
using PythonInterpreter.Visitors;
using FluentAssertions;
using System.Collections.Generic;
using PythonInterpreter.Extensions;
using PythonInterpreter.Tests.Helpers;

namespace PythonInterpreter.Tests
{
    public class MainVisitorTests
    {
        private readonly MainVisitor mainVisitor = new MainVisitor();

        #region Math statement
        public static IEnumerable<object[]> ShouldCorrectlyHandleMathStatement_Data => new List<object[]>
        {
            new object[] {"1 + 5", 6},
            new object[] {"1 + 5 + 4", 10},
            new object[] {"(1 + 5 + 4) * 2 + 5", 25},
            new object[] {"(1+ 5 +    4) * 2 + 5", 25}, // with different amount of spaces
            new object[] {"(1+5+4)*2+5", 25} // without spaces
        };

        [Theory]
        [MemberData(nameof(ShouldCorrectlyHandleMathStatement_Data))]
        public void ShouldCorrectlyHandleMathStatement(string expression, int expectedResult)
        {
            var result = mainVisitor.RunOnInput(expression);
            result.Should().Be(expectedResult);
        }
        #endregion
        #region Print function
        [Fact]
        public void ShouldCorrectlyPrintToConsole_WhenArgumentsAreTerminals()
        {
            // Arrange
            var testedStatement = "print(1,2,3)";
            // Act
            using(var mock = new ConsoleMock())
            {
                mainVisitor.RunOnInput(testedStatement);
                // Assert
                mock.Content.Should().Contain("1 2 3");
            };
        }

        [Fact]
        public void ShouldCorrectlyPrintToConsole_WhenArgumentsAreVariables()
        {
            // Arrange
            var testedStatement = @"
                a=1
                b= 2
                c = 3
                print(a,b,c)
            ";
            // Act
            using (var mock = new ConsoleMock())
            {
                mainVisitor.RunOnInput(testedStatement);
                // Assert
                mock.Content.Should().Contain("1 2 3");
            };
        }
        #endregion
        #region If statement
        [Fact]
        public void ShouldCorrectlyHandleIFStatement_WhenContidionIsTrue()
        {
            // Arrange
            var expectedResult = "12";
            var testedStatement = $@"
            if True:
                print({expectedResult})
            else:
                10
            end
            ";
            // Act
            using (var mock = new ConsoleMock())
            {
                mainVisitor.RunOnInput(testedStatement);
                // Assert
                mock.Content.Should().Contain(expectedResult);
            };
        }

        [Fact]
        public void ShouldCorrectlyHandleIFStatement_WhenContidionIsFalse()
        {
            // Arrange
            var expectedResult = "12";
            var testedStatement = $@"
            if False:
                10
            else:
               print({expectedResult})
            end
            ";
            // Act
            using (var mock = new ConsoleMock())
            {
                mainVisitor.RunOnInput(testedStatement);
                // Assert
                mock.Content.Should().Contain(expectedResult);
            };
        }

        [Fact]
        public void ShouldCorrectlyHandleIFStatement_WhenInCondition()
        {
            // Arrange
            var expectedResult = "12";
            var testedStatement = $@"
            if False:
                10
            elif True:
                print({expectedResult})
            end
            ";
            // Act
            using (var mock = new ConsoleMock())
            {
                mainVisitor.RunOnInput(testedStatement);
                // Assert
                mock.Content.Should().Contain(expectedResult);
            };
        }
        #endregion
    }
}
