using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.JavaScript;

namespace task04
{
    public interface ISpaceship
    {
        void MoveForward();      // Движение вперед
        void Rotate(int angle);  // Поворот на угол (градусы)
        void Fire();             // Выстрел ракетой
        int Speed { get; }       // Скорость корабля
        int FirePower { get; }   // Мощность выстрела
    }
    public class Cruiser: ISpaceship
    {
        public void MoveForward()
        {
            _way += Speed;
        }

        public void Rotate(int angle)
        {
            _angle += angle;
            while (_angle >= 360) _angle -= 360;
            while (_angle < 0) _angle += 360;
        }

        public void Fire()
        {
            _shots++;
        }
        public int Speed => 50; 
        public int FirePower => 100;
        int _way = 0;
        int _angle = 0;
        int _shots = 0;
        public int Way => _way;
        public int Angle => _angle;
        public int Shots => _shots;
        
    }
    public class Fighter: ISpaceship
    {
        public void MoveForward()
        {
            _way += Speed;
        }
        public void Rotate(int angle)
        {
            _angle += angle;
            while (_angle >= 360) _angle -= 360;
            while (_angle < 0) _angle += 360;
        }
        public void Fire()
        {
            _shots++;
        }
        public int Speed => 100;
        public int FirePower => 50;
        int _way = 0;
        int _angle = 0;
        int _shots = 0;
        public int Way => _way;
        public int Angle => _angle;
        public int Shots => _shots;
    }
}
