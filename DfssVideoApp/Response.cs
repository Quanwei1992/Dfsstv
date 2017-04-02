using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfssVideoApp
{
    public class BaseResponse
    {
        public int status;
        public string friendlyMessage;
        public string message;
        public string requestId;
        public int appOwnerId;
    }


    public class LoginResponse
    {
        public LoginResponseData data; 
        public int status;
        public string friendlyMessage;
        public string message;
        public string requestId;
        public int appOwnerId;
    }

    public class SubjectResponse
    {
        public SubjectData data;
        public int status;
        public string friendlyMessage;
        public string message;
        public string requestId;
        public int appOwnerId;
    }


    public class StartVideoResponse
    {
        public StartVideoData data;
        public int status;
        public string friendlyMessage;
        public string message;
        public string requestId;
        public int appOwnerId;
    }


    public class LoginResponseData
    {
        public string token;
        public DateTime created;
        public UserProfile userProfile;
    }


    public class SubjectData
    {
        public bool isFinished;
        public string finishedMsg;
        public Lesson[] lessons;
        public int subjectId;
        public string subjectName;
    }

    public class UserProfile
    {
        public string userName;
        public string name;
        public int userId;
        public string dlType;
        public string idNum;
        public int learningSubjectId;
    }


    //{"data":{"streamId":22511927},"status":0,"friendlyMessage":"成功！","message":"成功！","requestId":"21d608d48b0045179033c6ef039658e4","appOwnerId":1}

    public class StartVideoData
    {
        public string streamId;
    }


    /// <summary>
    /// 课程
    /// </summary>
    public class Lesson
    {
        public bool canLearn;
        public bool hasPractice;
        public bool isLearned;
        public string lessonName;
        public Practice practice;
        public int subjectId;
        public string teacherName;
        public string thumbnail;
        public Video[] videos;
    }

    /// <summary>
    /// 章节
    /// </summary>
    public class Practice
    {
        public Question[] questions;
    }

    public class Question
    {
        public int[] answer;
        public int id;
        public string image;
        public string imageHeight;
        public string imageWidth;
        public string[] options;
        public string question;    
    }


    /// <summary>
    /// 视频
    /// </summary>
    public class Video
    {
        public bool isWatched;
        public int lessonId;
        public int playOrder;
        public string playUrl;
        public int videoId;
        public string videoName;
        public int videoSeconds;
        public string videoSecondsTxt;
        public string videoThumbnail;
    }

}
