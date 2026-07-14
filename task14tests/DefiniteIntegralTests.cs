using System;
using Xunit;
using task14;

namespace task14tests
{
    public class DefiniteIntegralTests
    {
        [Fact]
        public void Solve_LinearFunction_FromMinus1To1_ReturnsZero()
        {
            double a = -1, b = 1;
            double step = 1e-4;
            int threads = 2;
            Func<double, double> f = x => x;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_SinFunction_FromMinus1To1_ReturnsZero()
        {
            double a = -1, b = 1;
            double step = 1e-5;
            int threads = 8;
            Func<double, double> f = Math.Sin;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(0, result, 1e-4);
        }

        [Fact]
        public void Solve_LinearFunction_From0To5_Returns12Point5()
        {
            double a = 0, b = 5;
            double step = 1e-6;
            int threads = 8;
            Func<double, double> f = x => x;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(12.5, result, 1e-5);
        }
        [Fact]
        public void Solve_OneThread_ShouldWorkCorrectly()
        {
            double a = 0, b = 5;
            double step = 1e-6;
            int threads = 1;
            Func<double, double> f = x => x;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(12.5, result, 1e-5);
        }

        [Fact]
        public void Solve_MoreThreadsThanSteps_ShouldNotBlock()
        {
            double a = 0, b = 1;
            double step = 0.5;
            int threads = 5;
            Func<double, double> f = x => x;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(0.5, result, 1e-6);
        }

        [Fact]
        public void Solve_ConstantFunction_ShouldReturnCorrectValue()
        {
            double a = 0, b = 3;
            double step = 1e-4;
            int threads = 4;
            Func<double, double> f = x => 5.0;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(15.0, result, 1e-4);
        }

        [Fact]
        public void Solve_QuadraticFunction_ShouldReturnCorrectValue()
        {
            double a = 0, b = 2;
            double step = 1e-5;
            int threads = 4;
            Func<double, double> f = x => x * x;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(8.0 / 3.0, result, 1e-5);
        }

        [Fact]
        public void Solve_DifferentThreadsCount_ShouldProduceSameResult()
        {
            double a = 0, b = 1;
            double step = 1e-5;
            Func<double, double> f = x => x;

            double result2 = DefiniteIntegral.Solve(a, b, f, step, 2);
            double result4 = DefiniteIntegral.Solve(a, b, f, step, 4);
            double result8 = DefiniteIntegral.Solve(a, b, f, step, 8);

            Assert.Equal(result2, result4, 1e-6);
            Assert.Equal(result4, result8, 1e-6);
        }
        [Fact]
        public void Solve_WithZeroSteps_ReturnsZero()
        {
            double a = 0, b = 1;
            double step = 10;
            int threads = 2;
            Func<double, double> f = x => x;

            double result = DefiniteIntegral.Solve(a, b, f, step, threads);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Solve_InvalidThreadsNumber_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => DefiniteIntegral.Solve(0, 1, x => x, 0.1, 0));
        }

        [Fact]
        public void Solve_InvalidStep_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => DefiniteIntegral.Solve(0, 1, x => x, -0.1, 2));
        }

        [Fact]
        public void Solve_InvalidBounds_ThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => DefiniteIntegral.Solve(1, 0, x => x, 0.1, 2));
        }

        [Fact]
        public void OptimalStep_ShouldMeetPrecision()
        {
            double a = -100, b = 100;
            Func<double, double> f = Math.Sin;
            double targetPrecision = 1e-4;
            double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };

            double reference = DefiniteIntegral.Solve(a, b, f, 1e-7, 8);

            foreach (var step in steps)
            {
                double value = DefiniteIntegral.Solve(a, b, f, step, 8);
                double error = Math.Abs(value - reference);

                if (error < targetPrecision)
                {
                    Assert.True(error < targetPrecision);
                    break;
                }
            }
        }
    }
}
