using System.Collections.Generic;
using PythonInterpreter.Visitors;
using PythonInterpreter.Extensions;
using System;
using Xunit;
using FluentAssertions;
using PythonInterpreter.Tests.Helpers;

namespace PythonInterpreter.Tests
{
    public class FunctionCallTests
    {
            private readonly MainVisitor mainVisitor = new MainVisitor();

            private static string TRUE => "True";
            private static string FALSE => "False";

            static public IEnumerable<object[]> ShouldPrintCorrectlyTestData => new List<object[]>
            {
                new object[] {TRUE, FALSE, FALSE, TestCase(1)},
                new object[] {FALSE, TRUE, FALSE, TestCase(2)},
                new object[] {FALSE, FALSE, TRUE, TestCase(3)},
            };
            private static string TestCase(int id) => $"Test case {id}";

            [Theory]
            [MemberData(nameof(ShouldPrintCorrectlyTestData))]
            public void ShouldPrintCorrectly_InElift(string cond1, string cond2, string cond3, string expectedPrint)
            {
                // Arrange
                var testString = @$"
                def foo2():
                    if {cond1}:
                        print(""{TestCase(1)}"")
                    elif {cond2}:
                        print(""{TestCase(2)}"")
                    elif {cond3}:
                        print(""{TestCase(3)}"")
                    end
                end
                foo2()";
            using (var mockConsole = new ConsoleMock())
            {
                mockConsole.Content.Should().BeEmpty();
                // Act 
                mainVisitor.RunOnInput(testString);
                // Assert
                mockConsole.Content.Should().Contain(expectedPrint);
            }
        }


        [Fact]
        public void ShouldReturnValue()
        {
            // Arrange
            var expectedResult = "1000";
            var testString = $@"
                def foo():
                    return {expectedResult}
                end
                a = foo()
                print(a)

                ";
            using (var mockConsole = new ConsoleMock())
            {
                // Act 
                mainVisitor.RunOnInput(testString);
                // Assert
                mockConsole.Content.Should().Contain(expectedResult);
            }
        }
    }
}
