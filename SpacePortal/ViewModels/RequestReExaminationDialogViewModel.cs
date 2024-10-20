﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using SpacePortal.Core.Models;


namespace SpacePortal.ViewModels
{
    public class RequestReExaminationDialogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public class AcademicYear
        {
            public string Year
            {
                get; set;
            }
            public ObservableCollection<string> Semesters
            {
                get; set;
            }

            public AcademicYear(string year)
            {
                Year = year;
                Semesters = new ObservableCollection<string>();
            }
        }

        public ObservableCollection<AcademicYear> Years { get; set; } = new ObservableCollection<AcademicYear>();
        public ObservableCollection<string> Subjects { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Semesters { get; set; } = new ObservableCollection<string>();
        private ObservableCollection<InformationsForGradesPage_GradesRow> _sourceData;

        public RequestReExaminationDialogViewModel(ObservableCollection<InformationsForGradesPage_GradesRow> sourceData)
        {
            _sourceData = sourceData;
            var sourceDataList = sourceData.ToList();
            var groupedData = sourceDataList.GroupBy(row => row.Year);

            foreach (var group in groupedData)
            {
                var academicYear = new AcademicYear(group.Key);
                foreach (var row in group)
                {
                    if (!academicYear.Semesters.Contains(row.Semester))
                    {
                        academicYear.Semesters.Add(row.Semester);
                    }
                }
                Years.Add(academicYear);
            }
        }

        private AcademicYear _selectedYear;


        public AcademicYear SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    UpdateSemesters();
                }
            }
        }

        private string _selectedSemester;

        public string SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                if (_selectedSemester != value)
                {
                    _selectedSemester = value;
                    UpdateSubjects();
                }
            }
        }

        private string _selectedSubject;

        public string SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                if (_selectedSubject != value)
                {
                    _selectedSubject = value;
                    UpdateScoreAndClass();
                }
            }
        }

        private void UpdateSemesters()
        {
            Semesters.Clear();
            if (_selectedYear != null)
            {
                foreach (var semester in _selectedYear.Semesters)
                {
                    Semesters.Add(semester);
                }
            }
        }

        private void UpdateSubjects()
        {
            Subjects.Clear();
            if (_selectedYear != null && !string.IsNullOrEmpty(_selectedSemester))
            {
                var filteredSubjects = _sourceData
                    .Where(row => row.Year == _selectedYear.Year && row.Semester == _selectedSemester)
                    .Select(row => row.CourseName)
                    .ToList();

                foreach (var subject in filteredSubjects)
                {
                    Subjects.Add(subject);
                }
            }
        }

        private void UpdateScoreAndClass()
        {
            if (!string.IsNullOrEmpty(SelectedSubject))
            {
                var selectedRow = _sourceData.FirstOrDefault(row => row.CourseName == SelectedSubject);
                if (selectedRow != null)
                {
                    Score = selectedRow.GradeScaleTen;
                    ClassID = selectedRow.ClassId;
                }
            }
            else
            {
                Score = 0;
                ClassID = string.Empty;
            }
        }

        public void SendRequest()
        {
            //TODO: Send request for re-examination
            Debug.WriteLine(SelectedYear?.Year);
            Debug.WriteLine(SelectedSemester);
            Debug.WriteLine(SelectedSubject);
            Debug.WriteLine(Score);
            Debug.WriteLine(ClassID);
            Debug.WriteLine(Note);
        }

        public int Score
        {
            get; set;
        }
        public string ClassID
        {
            get; set;
        }

        private string _note;
        public string Note
        {
            get => _note;
            set
            {
                if (_note != value)
                {
                    _note = value;
                }
            }
        }

    }
}

