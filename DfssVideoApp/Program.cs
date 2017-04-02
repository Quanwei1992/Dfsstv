using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DfssVideoApp
{

    public enum EStatus
    {
        Login,
        Run,
        Complate,
    }

    class Program
    {

        private static EStatus mStatus = EStatus.Login;
        private static DFSSAPIClient client = new DFSSAPIClient();
        private static bool IsQuit = false;
        static void Main(string[] args)
        {

            
           
            Console.WriteLine("欢迎使用东方时尚快速通过科目一课程工具");
            while (!IsQuit)
            {

                switch (mStatus)
                {
                    case EStatus.Login:
                        HandleLogin();
                    break;
                    case EStatus.Run:
                        HandleRun();
                        break;
                    case EStatus.Complate:
                        HandleComplate();
                        break;
                }

               



            }

            //client.Login("21027397","05020");
            Console.ReadKey();
        }

        static void HandleLogin()
        {
            Console.Write("账号:");
            string uid = Console.ReadLine();
            Console.Write("密码:");
            string pw = Console.ReadLine();
            var loginResponse = client.Login(uid, pw);
            if (loginResponse.status == 0)
            {
                Console.WriteLine("登陆成功!");
                Console.WriteLine("====================");
                Console.WriteLine("*姓名:" + loginResponse.data.userProfile.name);
                Console.WriteLine("*驾照类型:" + loginResponse.data.userProfile.dlType);
                Console.WriteLine("*当前学习学科ID:" + loginResponse.data.userProfile.learningSubjectId);
                Console.WriteLine("====================");
                mStatus = EStatus.Run;
            }
            else {
                Console.WriteLine("登陆失败:" + loginResponse.friendlyMessage);
            }
            
        }
        static void HandleRun()
        {
            Console.Write("正在获取学科数据...");
            var response = client.GetSubJect(client.SubjectID);
            Console.Write("done\n");
            if (response.status == 0)
            {
                Console.WriteLine("当前学科为:" + response.data.subjectName);
                if (!response.data.isFinished)
                {
                    Console.WriteLine("开始自动完成课程....");
                    var lessons = response.data.lessons;

                    foreach (var lesson in lessons)
                    {
                        if (lesson.isLearned) {
                            Console.WriteLine(lesson.lessonName+" 已学习过!");
                            continue;
                        }
                        var videos = lesson.videos;
                        Console.WriteLine("开始学习 " + lesson.lessonName);
                        Console.WriteLine(string.Format("共有{0}个视频，{1}道题", videos.Length, lesson.practice.questions.Length));
                        int lessonId = 0;
                        foreach (var video in videos)
                        {
                            lessonId = video.lessonId;
                            Console.Write("观看视频:" + video.videoName);
                            if (!video.isWatched)
                            {
                                var startRep = client.StartVideo(video.videoId.ToString());
                                if (startRep.status == 0)
                                {
                                    var endRep = client.EndVideo(startRep.data.streamId);
                                    if (endRep.status == 0)
                                    {
                                        Console.Write(" 完成\n");
                                    }
                                    else {
                                        Console.Write(" 失败," + endRep.friendlyMessage + "\n");
                                        mStatus = EStatus.Complate;
                                        return;
                                    }
                                }
                                else {
                                    Console.Write(" 失败," + startRep.friendlyMessage + "\n");
                                    mStatus = EStatus.Complate;
                                    return;
                                }
                            }
                            else {
                                Console.Write(" 完成\n");
                            }
                        }

                        Console.WriteLine("开始答题．．．");
                        var finishRep = client.FinishLesson(lessonId);
                        if (finishRep.status == 0)
                        {
                           Console.WriteLine("已完成:" + lesson.lessonName);
                        }
                        else {
                            Console.WriteLine("答题失败:" + finishRep.friendlyMessage);
                            mStatus = EStatus.Complate;
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine(response.data.finishedMsg);
                }
            }
            else {
                Console.WriteLine(response.friendlyMessage);
            }

            Console.WriteLine(response.friendlyMessage);
            mStatus = EStatus.Complate;
        }

        static void HandleComplate()
        {
            Console.Write("\n请按任意键退出");
            Console.ReadKey();
            IsQuit = true;
        }
    }
}
