namespace FrontEnd.Models
{
    public static class Constants
    {
        public static string LanguageBase = "ru-RU";
        public static string FullNamePrefix = "_FullName";
        public static string LanguagePrefix = "_Language";
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

        public static Dictionary<string, object> Dictionaries = new Dictionary<string, object>()
        {
            { "ru-RU", new CaptionsBase() },
            { "en-US", new CaptionsEN() }
        };
    }

    public class CaptionsBase
    {
        public string CorporationSocialSystem = "Корпоративная социальная система";
        public string TitleHomeCaption = "Главная";
        public string PageTitleHomeCaption = "Корпоративная социальная система";
        public string GoToNewsCaption = "Перейти к новостям";
        public string TitleNewsCaption = "Новости";
        public string PageTitleNewsCaption = "Новости компании";
        public string PageTitleSearchNewsCaption = "Поиск по новостям";
        public string PageTitleCreatingNewsCaption = "Создание новости";
        public string GoToCreatingNewsCaption = "Перейти к созданию новости";
        public string SearchPlainFormTitleCaption = "Тема (частично)";
        public string SearchPlainFormAuthorNameCaption = "Имя автора";
        public string SearchPlainFormAuthorSurnameCaption = "Фамилия автора";
        public string SearchPlainFormCreatedAtStartCaption = "Дата создания С";
        public string SearchPlainFormCreatedAtTillCaption = "Дата создания ПО";
        public string SearchPlainFormHashtagsCaption = "Хэштеги (через пробел без решетки)";
        public string SearchButtonCaption = "Искать";
        public string MoreButtonCaption = "Подробнее";
        public string HideButtonCaption = "Скрыть";
        public string LoadMoreCaption = "Больше";
        public string CreateNewsCaption = "Создать новость";
        public string SearchNewsCaption = "Поиск";
        public string WideSearchNewsCaption = "Расширенный поиск";
        public string CreatingNewsTitleCaption = "Тема";
        public string CreatingNewsShortDescriptionCaption = "Краткое описание";
        public string CreatingNewsContentCaption = "Описание";
        public string CreatingNewsHashtagsCaption = "Хэштеги (через пробел без решетки)";
        public string CreateButtonCaprion = "Создать";
        public string TitleCreatedNewsCaption = "Новость успешно создана";
        public string PageTitleCreatedNewsCaption = "Новость создана";
        public string ItWillBeShownAfterModerationCaption = "После прохождения модерации она отобразится в ленте новостей.";
        public string TitleDeletedNewsCaption = "Новость успешно удалена";
        public string PageTitleDeletedNewsCaption = "Новость удалена";
        public string TitleUpdatedNewsCaption = "Новость успешно обновлена";
        public string PageTitleUpdatedNewsCaption = "Новость обновлена";
        public string TitleFailedCreatingNewsCaption = "Ошибка при создании новости";
        public string PageTitleFailedCreatingNewsCaption = "Ошибка";
        public string ErrorTillCreatingNewsCaption = "Во время создания новости произошла ошибка. Попробуйте позднее.";
        public string TitleFailedDeletingNewsCaption = "Ошибка при удалении новости";
        public string PageTitleFailedDeletingNewsCaption = "Ошибка";
        public string ErrorTillDeletingNewsCaption = "Во время удаления новости произошла ошибка.Попробуйте позднее.";
        public string TitleFailedUpdatingNewsCaption = "Ошибка при обновлении новости";
        public string PageTitleFailedUpdatingNewsCaption = "Ошибка";
        public string ErrorTillUpdatingNewsCaption = "Во время обновления новости произошла ошибка.Попробуйте позднее.";
        public string PageTitleUpdatingNewsCaption = "Редактирование новости";
        public string DeleteNewsQuestionCaption = "Удалить новость?";
        public string NoCaption = "Нет";
        public string YesCaption = "Да";
        public string DeleteNewsButtonCaption = "Удалить новость";
        public string SaveButtonCaption = "Сохранить";
        public string GuestCaption = "Гость";
        public string ProfileCaption = "Профиль";
        public string ExitCaption = "Выход";
        public string PersonalAccountMenuCaption = "Личный кабинет";
        public string NewsFeedMenuCaption = "Новости";
        public string TimesheetMenuCaption = "Таймшит";
    }

    public class CaptionsEN : CaptionsBase
    {

    }
}
