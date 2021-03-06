﻿using GeneralTools.Contracts;

namespace GeneralTools.Services
{
    public class LocalizationService : ILocalizationService
    {
        #region Privates and constructor

        private readonly ITranslationFormatService _translationFormatService;

        public LocalizationService(ITranslationFormatService translationFormatService)
        {
            _translationFormatService = translationFormatService;
        }

        #endregion

        #region ILocalizationService Implementierung

        public string TranslateResourceKey(string resource)
        {
            return _translationFormatService.GetTranslation(resource);
        }

        #endregion
    }
}
