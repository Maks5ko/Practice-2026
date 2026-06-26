using task04;
using Xunit;
public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }
    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }
    [Fact]
    public void Cruiser_ShouldHaveMoreFirePowerThanFighter()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(cruiser.FirePower > fighter.FirePower);
    }


    [Fact]
    public void Cruiser_DefaultIsZero()
    {
        var cruiser = new Cruiser();
        Assert.Equal(0, cruiser.Way);
        Assert.Equal(0, cruiser.Angle);
        Assert.Equal(0, cruiser.Shots);
    }

    [Fact]
    public void Cruiser_MoveForward_Correctly()
    {
        var cruiser = new Cruiser();
        int initialWay = cruiser.Way;
        cruiser.MoveForward();
        Assert.Equal(initialWay + cruiser.Speed, cruiser.Way);
    }
    [Fact]
    public void Cruiser_Rotate_ChangesAngleCorrectly()
    {
        var cruiser = new Cruiser();
        cruiser.Rotate(30);
        Assert.Equal(30, cruiser.Angle);
        cruiser.Rotate(-60);
        Assert.Equal(330, cruiser.Angle);
        cruiser.Rotate(400);
        Assert.Equal(10, cruiser.Angle);
        cruiser.Rotate(-1000);
        Assert.Equal(90, cruiser.Angle);
    }
    [Fact]
    public void Cruiser_Fire_Correctly()
    {
        var cruiser = new Cruiser();
        int initialShots = cruiser.Shots;
        cruiser.Fire();
        Assert.Equal(initialShots + 1, cruiser.Shots);
    }


    [Fact]
    public void Fighter_DefaultStateIsZero()
    {
        var fighter = new Fighter();
        Assert.Equal(0, fighter.Way);
        Assert.Equal(0, fighter.Angle);
        Assert.Equal(0, fighter.Shots);
    }
    [Fact]
    public void Fighter_MoveForward_Correctly()
    {
        var fighter = new Fighter();
        int initialWay = fighter.Way;
        fighter.MoveForward();
        Assert.Equal(initialWay + fighter.Speed, fighter.Way);
    }
    [Fact]
    public void Fighter_Rotate_ChangesAngleCorrectly()
    {
        var fighter = new Fighter();
        fighter.Rotate(30);
        Assert.Equal(30, fighter.Angle);
        fighter.Rotate(-60);
        Assert.Equal(330, fighter.Angle);
        fighter.Rotate(400);
        Assert.Equal(10, fighter.Angle);
        fighter.Rotate(-1000);
        Assert.Equal(90, fighter.Angle);
    }
    [Fact]
    public void Fighter_Fire_Correctly()
    {
        var fighter = new Fighter();
        int initialShots = fighter.Shots;
        fighter.Fire();
        Assert.Equal(initialShots + 1, fighter.Shots);
    }
}
