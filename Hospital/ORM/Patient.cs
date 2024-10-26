﻿namespace Hospital.ORM
{
    public class Patient
    {
        public int Id { get; set; }
        public string SecondName { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }

        public int AreaId { get; set; }
        public Area Area { get; set; }
    }
}