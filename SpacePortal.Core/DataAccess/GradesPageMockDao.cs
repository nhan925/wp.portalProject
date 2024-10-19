using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpacePortal.Core.Contracts;
using SpacePortal.Core.Models;

namespace SpacePortal.Core.DataAccess;
public class GradesPageMockDao : IDao<GradesRow>
{
    public void Add(GradesRow entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public ObservableCollection<GradesRow> GetAll()
    {
        var result = new ObservableCollection<GradesRow>
        {
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "1",
                CourseId = "COSC 1436",
                CourseName = "Programming Fundamentals I",
                CourseCredit = 4,
                ClassID = "12345",
                GradeScaleTen = 9,
                GradeScaleFour = 4,
                Note = "Excellent"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "1",
                CourseId = "COSC 1437",
                CourseName = "Programming Fundamentals II",
                CourseCredit = 4,
                ClassID = "12346",
                GradeScaleTen = 8,
                GradeScaleFour = 3,
                Note = "Good"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "1",
                CourseId = "COSC 2436",
                CourseName = "Data Structures",
                CourseCredit = 4,
                ClassID = "12347",
                GradeScaleTen = 7,
                GradeScaleFour = 2,
                Note = "Fair"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "1",
                CourseId = "COSC 2437",
                CourseName = "Database Systems",
                CourseCredit = 4,
                ClassID = "12348",
                GradeScaleTen = 6,
                GradeScaleFour = 1,
                Note = "Poor"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "1",
                CourseId = "COSC 3336",
                CourseName = "Operating Systems",
                CourseCredit = 4,
                ClassID = "12349",
                GradeScaleTen = 5,
                GradeScaleFour = 0,
                Note = "Fail"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "2",
                CourseId = "COSC 3337",
                CourseName = "Software Engineering",
                CourseCredit = 4,
                ClassID = "12350",
                GradeScaleTen = 4,
                GradeScaleFour = 0,
                Note = "Fail"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "2",
                CourseId = "COSC 4336",
                CourseName = "Computer Networks",
                CourseCredit = 4,
                ClassID = "12351",
                GradeScaleTen = 3,
                GradeScaleFour = 0,
                Note = "Fail"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "2",
                CourseId = "COSC 4337",
                CourseName = "Web Development",
                CourseCredit = 4,
                ClassID = "12352",
                GradeScaleTen = 2,
                GradeScaleFour = 0,
                Note = "Fail"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "2",
                CourseId = "COSC 5336",
                CourseName = "Artificial Intelligence",
                CourseCredit = 4,
                ClassID = "12353",
                GradeScaleTen = 1,
                GradeScaleFour = 0,
                Note = "Fail"
            },
            new GradesRow
            {
                Year = "2021 - 2022",
                Semester = "2",
                CourseId = "COSC 5337",
                CourseName = "Machine Learning",
                CourseCredit = 4,
                ClassID = "12354",
                GradeScaleTen = 0,
                GradeScaleFour = 0,
                Note = "Fail"
            }
        };
        return result;
    }

    public GradesRow GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(GradesRow entity)
    {
        throw new NotImplementedException();
    }

}
