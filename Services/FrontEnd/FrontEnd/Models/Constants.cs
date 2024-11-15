namespace FrontEnd.Models
{
    public static class Constants
    {
        public static string LanguageBase = "ru-RU";
        public static string FullNamePrefix = "_FullName";
        public static string LanguagePrefix = "_Language";
        public static string ObjectPrefix = "_Object";
        public static string IsAdminPrefix = "_IsAdmin";
        public static string NewsFeedListViewDataKey = "NewsList";
        public static string UserFullNameKey = "UserFullName";
        public static string PersonalAccountDataKey = "PersonalAccountData";
        public static string UserIdCookieKey = "userId";
        public static string NewsListViewModelKey = "NewsListViewModel";
        public static string CaptionsKey = "Captions";
        public static string EmojiViewModelListKey = "EmojiViewModelList";
        public static string CreateNewsViewModelKey = "CreateNewsViewModel";
        public static string LangRUCaption = "RU";
        public static string LangENCaption = "EN";
        public static string RuRULangCaption = "ru-RU";
        public static string EnUSLangCaption = "en-US";
        public static string LanguageKey = "Langs";

        public static Dictionary<string, object> Dictionaries = new Dictionary<string, object>()
        {
            { "ru-RU", new CaptionsBase() },
            { "en-US", new CaptionsEN() }
        };

        public static Dictionary<string, string> Langs = new Dictionary<string, string>()
        {
            { "ru-RU", LangRUCaption },
            { "en-US", LangENCaption }
        };
    }

    public class CaptionsBase
    {
        public virtual string CorporationSocialSystem { get; } = "Корпоративная социальная система";
        public virtual string TitleHomeCaption { get; } = "Главная";
        public virtual string PageTitleHomeCaption { get; } = "Корпоративная социальная система";
        public virtual string GoToNewsCaption { get; } = "Перейти к новостям";
        public virtual string TitleNewsCaption { get; } = "Новости";
        public virtual string PageTitleNewsCaption { get; } = "Новости компании";
        public virtual string PageTitleSearchNewsCaption { get; } = "Поиск по новостям";
        public virtual string PageTitleCreatingNewsCaption { get; } = "Создание новости";
        public virtual string GoToCreatingNewsCaption { get; } = "Перейти к созданию новости";
        public virtual string SearchPlainFormTitleCaption { get; } = "Тема (частично)";
        public virtual string SearchPlainFormAuthorNameCaption { get; } = "Имя автора";
        public virtual string SearchPlainFormAuthorSurnameCaption { get; } = "Фамилия автора";
        public virtual string SearchPlainFormCreatedAtStartCaption { get; } = "Дата создания С";
        public virtual string SearchPlainFormCreatedAtTillCaption { get; } = "Дата создания ПО";
        public virtual string SearchPlainFormHashtagsCaption { get; } = "Хэштеги (через пробел без решетки)";
        public virtual string SearchButtonCaption { get; } = "Искать";
        public virtual string MoreButtonCaption { get; } = "Подробнее";
        public virtual string HideButtonCaption { get; } = "Скрыть";
        public virtual string LoadMoreCaption { get; } = "Больше";
        public virtual string CreateNewsCaption { get; } = "Создать новость";
        public virtual string SearchNewsCaption { get; } = "Поиск";
        public virtual string WideSearchNewsCaption { get; } = "Расширенный поиск";
        public virtual string CreatingNewsTitleCaption { get; } = "Тема";
        public virtual string CreatingNewsShortDescriptionCaption { get; } = "Краткое описание";
        public virtual string CreatingNewsContentCaption { get; } = "Описание";
        public virtual string CreatingNewsHashtagsCaption { get; } = "Хэштеги (через пробел без решетки)";
        public virtual string CreateButtonCaprion { get; } = "Создать";
        public virtual string TitleCreatedNewsCaption { get; } = "Новость успешно создана";
        public virtual string PageTitleCreatedNewsCaption { get; } = "Новость создана";
        public virtual string ItWillBeShownAfterModerationCaption { get; } = "После прохождения модерации она отобразится в ленте новостей.";
        public virtual string TitleDeletedNewsCaption { get; } = "Новость успешно удалена";
        public virtual string PageTitleDeletedNewsCaption { get; } = "Новость удалена";
        public virtual string TitleUpdatedNewsCaption { get; } = "Новость успешно обновлена";
        public virtual string PageTitleUpdatedNewsCaption { get; } = "Новость обновлена";
        public virtual string TitleFailedCreatingNewsCaption { get; } = "Ошибка при создании новости";
        public virtual string PageTitleFailedCreatingNewsCaption { get; } = "Ошибка";
        public virtual string ErrorTillCreatingNewsCaption { get; } = "Во время создания новости произошла ошибка. Попробуйте позднее.";
        public virtual string TitleFailedDeletingNewsCaption { get; } = "Ошибка при удалении новости";
        public virtual string PageTitleFailedDeletingNewsCaption { get; } = "Ошибка";
        public virtual string ErrorTillDeletingNewsCaption { get; } = "Во время удаления новости произошла ошибка.Попробуйте позднее.";
        public virtual string TitleFailedUpdatingNewsCaption { get; } = "Ошибка при обновлении новости";
        public virtual string PageTitleFailedUpdatingNewsCaption { get; } = "Ошибка";
        public virtual string ErrorTillUpdatingNewsCaption { get; } = "Во время обновления новости произошла ошибка.Попробуйте позднее.";
        public virtual string PageTitleUpdatingNewsCaption { get; } = "Редактирование новости";
        public virtual string DeleteNewsQuestionCaption { get; } = "Удалить новость?";
        public virtual string NoCaption { get; } = "Нет";
        public virtual string YesCaption { get; } = "Да";
        public virtual string DeleteNewsButtonCaption { get; } = "Удалить новость";
        public virtual string SaveButtonCaption { get; } = "Сохранить";
        public virtual string GuestCaption { get; } = "Гость";
        public virtual string ProfileCaption { get; } = "Профиль";
        public virtual string ExitCaption { get; } = "Выход";
        public virtual string PersonalAccountMenuCaption { get; } = "Личный кабинет";
        public virtual string NewsFeedMenuCaption { get; } = "Новости";
        public virtual string TimesheetMenuCaption { get; } = "Таймшит";
        public virtual string TitlePersonalAccountCaption { get; } = "Профиль";
        public virtual string PageTitlePersonalAccountCaption { get; } = "Личный кабинет";
        public virtual string EmployeeNameCaption { get; } = "Имя";
        public virtual string EmployeeSurnameCaption { get; } = "Фамилия";
        public virtual string EmployeeBirthdayCaption { get; } = "Дата рождения";
        public virtual string EmployeeEmailCaption { get; } = "E-mail";
        public virtual string EmployeeOfficeCaption { get; } = "Адрес работы";
        public virtual string EmployeePositionCaption { get; } = "Должность";
        public virtual string EmployeePhoneCaption { get; } = "Телефон";
        public virtual string EmployeeAboutCaption { get; } = "О себе";
        public virtual string EmployeeEmploymentDateCaption { get; } = "Дата начала работы";
		public virtual string PAProfileCaption { get; } = "Профиль";
		public virtual string PAMineNewsCaption { get; } = "Мои новости";
		public virtual string PAModerationNewsCaption { get; } = "На модерации";
		public virtual string PAArchivedNewsCaption { get; } = "Архив";
		public virtual string PALikedNewsCaption { get; } = "Понравившееся";
	}

    public class CaptionsEN : CaptionsBase
    {
        public override string CorporationSocialSystem { get; } = "Corporation social system";
        public override string TitleHomeCaption { get; } = "Main";
        public override string PageTitleHomeCaption { get; } = "Corporation social system";
        public override string GoToNewsCaption { get; } = "Go to news";
        public override string TitleNewsCaption { get; } = "News";
        public override string PageTitleNewsCaption { get; } = "Company news";
        public override string PageTitleSearchNewsCaption { get; } = "News search";
    }
}
