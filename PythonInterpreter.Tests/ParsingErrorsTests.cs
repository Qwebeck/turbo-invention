using PythonInterpreter.Visitors;
using PythonInterpreter.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace PythonInterpreter.Tests
{
    public class IFStatementParsingErrorsTests
    {
        private readonly MainVisitor mainVisitor = new MainVisitor();

        [Fact]
        public void ShouldThrow_When_EndIsMissing()
        {
            // Arrange
            var testString = @"
            if True:
                print(1)
            ";
            Action action = () => mainVisitor.RunOnInput(testString);
            // Act, Assert
            action.Should().Throw<BadParserInputException>();

        }

        [Fact]
        public void ShouldThrow_When_ConditionIsMissing()
        {
            // Arrange
            var testString = @"
            if :
                print(1)
            end
            ";
            Action action = () => mainVisitor.RunOnInput(testString);
            // Act, Assert
            action.Should().Throw<BadParserInputException>();
        }

        [Fact]
        public void ShouldThrow_When_ColonIsMissing()
        {
            // Arrange
            var testString = @"
            if True
                print(1)
            end    
            ";
            Action action = () => mainVisitor.RunOnInput(testString);
            // Act, Assert
            action.Should().Throw<BadParserInputException>();
        }

        [Fact]
        public void ShouldNotThrow_When_IfIsCorrect()
        {
            // Arrange
            var testString = @"
            if True:
                print(1)
            end
            ";
            Action action = () => mainVisitor.RunOnInput(testString);
            // Act, Assert
            action.Should().NotThrow();
        }
 
    }
}
