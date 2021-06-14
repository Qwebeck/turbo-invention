using PythonInterpreter.Visitors;
using PythonInterpreter.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace PythonInterpreter.Tests
{
    public class FunctionDefinitionStatementParsingErrorsTests
    {
        private readonly MainVisitor mainVisitor = new MainVisitor();

        [Fact]
        public void ShouldThrow_When_ParanthesisiAreMissing()
        {
            // Arrange
            var testString = @"
            def foo:
                print(1)
            end
            ";
            Action action = () => mainVisitor.RunOnInput(testString);
            // Act, Assert
            action.Should().Throw<BadParserInputException>();

        }

        [Fact]
        public void ShouldThrow_When_EndIsMissing()
        {
            // Arrange
            var testString = @"
            def foo():
                print(1)
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
            def foo()
                print(1)
            end    
            ";
            Action action = () => mainVisitor.RunOnInput(testString);
            // Act, Assert
            action.Should().Throw<BadParserInputException>();
        }

        [Fact]
        public void ShouldNotThrow_When_IsCorrect()
        {
            // Arrange
            var testString = @"
            def foo(a):
                print(a)
            end
            ";
            Action action = () => mainVisitor.RunOnInput(testString);
            // Act, Assert
            action.Should().NotThrow();
        }

    }
}
