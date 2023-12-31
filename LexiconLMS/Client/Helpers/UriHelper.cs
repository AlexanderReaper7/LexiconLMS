namespace LexiconLMS.Client.Helpers
{
    public static class UriHelper
    {
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

		public static string GetActivitiesUri()
		{
			return ActivitiesBaseUri;
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
	}
}
