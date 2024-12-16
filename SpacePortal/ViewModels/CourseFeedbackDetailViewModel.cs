using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Models;
using Windows.ApplicationModel.Resources;
namespace SpacePortal.ViewModels;

public partial class CourseFeedbackDetailViewModel : ObservableRecipient
{
    public CourseFeedbackDetailInformations Informations { get; set; } =  new CourseFeedbackDetailInformations();

    public ObservableCollection<Question> CourseQuesntion { get; set; } = new ObservableCollection<Question>();

    public ObservableCollection<Question> TeacherQuestion { get; set; } = new ObservableCollection<Question>();


    public void LoadInformations(string courseName,string courseID, string teacherName, string classID)
    {
        Informations.courseName = courseName;
        Informations.courseID = courseID;
        Informations.teacherName = teacherName;
        Informations.classID = classID;
    }

    public void LoadCourseQuesntion()
    {
        ResourceLoader resourceLoader = new();
        for (var i = 1; i <= 5 ; i++)
        {
            var questionContent = resourceLoader.GetString($"CourseFeedbackDetail_CourseQuestion0{i}/Text");
            CourseQuesntion.Add(new Question { Number = i, Content = questionContent });
        }
    }

    public void LoadTeacherQuesntion()
    {
        ResourceLoader resourceLoader = new();
        for (var i = 1; i <= 5; i++)
        {
            var questionContent = resourceLoader.GetString($"CourseFeedbackDetail_TeacherQuestion0{i}/Text");
            TeacherQuestion.Add(new Question { Number = i, Content = questionContent });
        }
    }
}
