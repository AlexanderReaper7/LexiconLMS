namespace LexiconLMS.Client.Helpers
{
    public static class UriHelper
    {
        #region client's URIs
        private const string course = "course";
        private const string module = "module";
        private const string activity = "activity";
		private const string assignment = "assignment";
		private const string add = "add";
		private const string update = "update";
		private const string delete = "delete";
		private const string details = "details";
		private const string list = "list";

		public const string CourseList = $"/{list}of{course}s";
		public const string CourseDetails = $"/{course}/{{courseId}}";
		public const string CourseUpdate = $"/{course}{update}/{{courseId}}";
		public const string CourseAdd = $"/{course}{add}";
		public const string CourseDelete = $"/{course}{delete}/{{courseId}}";

		public const string ModuleDetails = $"/{module}{details}/{{moduleId:guid}}";
		public const string ModuleUpdate = $"/{module}{update}/{{moduleId:guid}}";
		public const string ModuleAdd = $"/{module}{add}/{{courseId:guid}}";
		public const string ModuleDelete = $"/{module}{delete}/{{moduleId:guid}}";

		public const string ActivityList = $"/activities{list}";
		public const string ActivityDetails = $"/{activity}{details}/{{activityId:guid}}";
		public const string ActivityUpdate = $"/{activity}{update}/{{activityId:guid}}";
		public const string ActivityAdd = $"/{activity}{add}/{{moduleId:guid}}";
		public const string ActivityDelete = $"/{activity}{delete}/{{activityId:guid}}";

		public const string AssignmentDetails = $"/{assignment}{details}/{{activityId:guid}}";
		public const string AssignmentUpdate = $"/{assignment}{update}/{{activityId:guid}}";
		public const string AssignmentAdd = $"/{assignment}{add}/{{moduleId:guid}}";

		//Course uri
		public static string GetCourseAddUri() =>  CourseAdd;
        public static string GetCourseUpdateUri<T>(T courseId) =>  $"/{course}{update}/{courseId}";
        public static string GetCourseDeleteUri<T>(T courseId) =>  $"/{course}{delete}/{courseId}";
        public static string GetCourseDetailsUri<T>(T courseId) =>  $"/{course}/{courseId}";
        public static string GetCourseListUri() => CourseList;

		//Module uri
		public static string GetModuleAddUri<T>(T courseId) => $"/{module}{add}/{courseId}";
		public static string GetModuleUpdateUri<T>(T moduleId) =>  $"/{module}{update}/{moduleId}";
		public static string GetModuleDeleteUri<T>(T moduleId) =>  $"/{module}{delete}/{moduleId}";
		public static string GetModuleDetailsUri<T>(T moduleId)=>  $"/{module}{details}/{moduleId}";

		//Assignment uri
		public static string GetActivityAddUri<T>(T moduleId) => $"/{activity}{add}/{moduleId}";
		public static string GetActivityUpdateUri<T>(T activityId) =>  $"/{activity}{update}/{activityId}";
		public static string GetActivityDeleteUri<T>(T activityId) =>  $"/{activity}{delete}/{activityId}";
		public static string GetActivityDetailsUri<T>(T activityId)=>  $"/{activity}{details}/{activityId}";
		public static string GetActivityListUri()=>  ActivityList;
		
		//Assignment uri
		public static string GetAssignmentAddUri<T>(T moduleId) => $"/{assignment}{add}/{moduleId}";
		public static string GetAssignmentUpdateUri<T>(T assignmentId) =>  $"/{assignment}{update}/{assignmentId}";
		public static string GetAssignmentDetailsUri<T>(T assignmentId) => $"/{assignment}{details}/{assignmentId}";
        #endregion

        #region API's URIs
        public static string ModulesBaseUri => "api/modules/";

        public static string GetModuleUri<T>(T moduleId, bool includeActivities = false)
        {
            return ModulesBaseUri + moduleId!.ToString() + $"?includeActivities={includeActivities}";
        }

        public static string GetModulesUri()
        {
            return ModulesBaseUri;
        }

        public static string ActivitiesBaseUri => "api/activities/";

		public static string GetActivityUri<T>(T activityId)
		{
			return ActivitiesBaseUri + activityId!.ToString();
		}

		public static string GetActivitiesUri(string? moduleId = null)
		{
			string query = string.Empty;

            if (moduleId is not null)
            {
				query = $"moduleId={moduleId}";
            }

			if (query != "")
			{
				return ActivitiesBaseUri + $"?{query}";
			}

			return ActivitiesBaseUri;
		}

		public static string ActivityDocumentsBaseUri => "api/ActivityDocuments/";

		public static string GetActivityDocumentsUri(string? activityId = null)
		{
			string query = string.Empty;

			if (activityId is not null)
			{
				query = $"activityId={activityId}";
			}

			if (query != "")
			{
				return ActivityDocumentsBaseUri + $"?{query}";
			}

			return ActivityDocumentsBaseUri;
		}

		public static string GetAssignmentsTeachersUri<TCourseId, TModuleId>(TCourseId courseId, TModuleId moduleId)
		{
            return $"{ActivitiesBaseUri}courses/{courseId}/modules/{moduleId}/assignments";
        }

		public static string GetAssignmentsStudentsUri<TModuleId, TStudentId>(TModuleId moduleId, TStudentId studentId)
		{
			return $"{ActivitiesBaseUri}modules/{moduleId}/assignments/{studentId}";
		}
        public static string GetAssignmentStudentsUri<TStudentId, TAssignmentId>(TStudentId studentId, TAssignmentId assignmentId)
        {
			var s = $"{ActivitiesBaseUri}students/{studentId}/assignments/{assignmentId}";

			return s;
        }

        public static string CoursesBaseUri => "api/Courses/";

		public static string GetCourseUri<T>(T courseId)
		{
			return CoursesBaseUri + courseId!.ToString();
		}

		public static string GetCoursesUri()
		{
			return CoursesBaseUri;
		}

		public static string ActivityTypesBaseUri => "api/activityTypes/";

		public static string GetActivityTypeUri<T>(T activityTypeId)
		{
			return ActivityTypesBaseUri + activityTypeId!.ToString();
		}

		public static string GetActivityTypesUri()
		{
			return ActivityTypesBaseUri;
		}
		
		public static string ApplicationUserBaseUri => "api/applicationuser/";

		public static string GetApplicationUserUri<T>(T applicationUserId)
		{
			return ApplicationUserBaseUri + applicationUserId!.ToString();
		}
		public static string GetApplicationUserByNameUri<T>(T applicationUserName)
		{
			return ApplicationUserBaseUri + "byname/" + applicationUserName!.ToString();
		}

		public static string GetApplicationUsersUri()
		{
			return ApplicationUserBaseUri;
		}
        #endregion
    }
}
