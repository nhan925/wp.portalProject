using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SpacePortal.Core.Services;
using SpacePortal.Models;
using Windows.ApplicationModel.Resources;
namespace SpacePortal.ViewModels;

public partial class CourseFeedbackDetailViewModel : ObservableRecipient
{
    public CourseFeedbackDetailInformations Informations { get; set; } =  new CourseFeedbackDetailInformations();

    public ObservableCollection<Question> CourseQuestion { get; set; } = new ObservableCollection<Question>();

    public ObservableCollection<Question> TeacherQuestion { get; set; } = new ObservableCollection<Question>();


    public void LoadInformations(string courseName,string courseID, string teacherName, string classID)
    {
        Informations.courseName = courseName;
        Informations.courseID = courseID;
        Informations.teacherName = teacherName;
        Informations.classID = classID;
    }

    public bool UploadFeedbackToDatabase()
    {
        var message = CreateFeedbackMessage();
        var info = new
        {
            v_class_id = Informations.classID,
            v_feedback = message
        };
        var result = App.GetService<ApiService>().Post<string>("/rpc/add_feedback_course",info);
        if(result == "Success")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckEmptyField()
    {
        foreach (var question in CourseQuestion)
        {
            if (question.SelectedRating == 0)
            {
                return false;
            }
        }

        foreach (var question in TeacherQuestion)
        {
            if (question.SelectedRating == 0)
            {
                return false;
            }
        }
        return true;
    }

    public string CreateFeedbackMessage()
    {
        var courseFeedbackMessage = "Đánh giá môn học: ";
        var teacherFeedbackMessage = "Đánh giá giảng viên: ";
        for (var i = 0; i < 5; i++)
        {
            courseFeedbackMessage += $"Câu hỏi: {i +1} - {CourseQuestion[i].SelectedRating} ";
            teacherFeedbackMessage += $"Câu hỏi: {i + 1} - {TeacherQuestion[i].SelectedRating} ";
        }

        var fullFeedbackMessage = courseFeedbackMessage + " | " + teacherFeedbackMessage;
        return fullFeedbackMessage;
    }

    public void LoadCourseQuestion()
    {
        ResourceLoader resourceLoader = new();
        for (var i = 1; i <= 5 ; i++)
        {
            var questionContent = resourceLoader.GetString($"CourseFeedbackDetail_CourseQuestion0{i}/Text");
            CourseQuestion.Add(new Question { Number = i, Content = questionContent });
        }
    }

    public void LoadTeacherQuestion()
    {
        ResourceLoader resourceLoader = new();
        for (var i = 1; i <= 5; i++)
        {
            var questionContent = resourceLoader.GetString($"CourseFeedbackDetail_TeacherQuestion0{i}/Text");
            TeacherQuestion.Add(new Question { Number = i + 5, Content = questionContent });
        }
    }
}
