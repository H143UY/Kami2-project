//
//  AppLovinInternalSettigns.cs
//  AppLovin User Engagement Unity Plugin
//
//  Created by Santosh Bagadi on 9/15/22.
//  Copyright © 2022 AppLovin. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AppLovinMax.Scripts.IntegrationManager.Editor
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> representing the AppLovin internal settings that can be set in the Integration Manager Window.
    ///
    /// The scriptable object asset is saved under ProjectSettings as <c>AppLovinInternalSettings.json</c>.
    /// </summary>
    public class AppLovinInternalSettings : ScriptableObject
    {
        private static AppLovinInternalSettings _instance;

        private const string DefaultUserTrackingDescriptionEn = "This uses device info for more personalized ads and content";
        private const string DefaultUserTrackingDescriptionDe = "Dies benutzt Gerätinformationen für relevantere Werbeinhalte";
        private const string DefaultUserTrackingDescriptionEs = "Esto utiliza la información del dispositivo para anuncios y contenido más personalizados";
        private const string DefaultUserTrackingDescriptionFr = "Cela permet d'utiliser les informations du téléphone pour afficher des contenus publicitaires plus pertinents.";
        private const string DefaultUserTrackingDescriptionJa = "これはユーザーデータをもとに、より関連性の高い広告コンテンツをお客様に提供します";
        private const string DefaultUserTrackingDescriptionKo = "보다 개인화된 광고 및 콘텐츠를 위해 기기 정보를 사용합니다.";
        private const string DefaultUserTrackingDescriptionZhHans = "我们使用设备信息来提供个性化的广告和内容。";
        private const string DefaultUserTrackingDescriptionZhHant = "我們使用設備信息來提供個性化的廣告和內容。";

        [SerializeField] private bool consentFlowEnabled;
        [SerializeField] private string consentFlowPrivacyPolicyUrl = string.Empty;
        [SerializeField] private string consentFlowTermsOfServiceUrl = string.Empty;
        [SerializeField] private bool shouldShowTermsAndPrivacyPolicyAlertInGDPR;
        [SerializeField] private bool overrideDefaultUserTrackingUsageDescriptions;
        [SerializeField] private MaxSdkBase.ConsentFlowUserGeography debugUserGeography;
        [SerializeField] private string userTrackingUsageDescriptionEn = string.Empty;
        [SerializeField] private bool userTrackingUsageLocalizationEnabled;
        [SerializeField] private string userTrackingUsageDescriptionDe = string.Empty;
        [SerializeField] private string userTrackingUsageDescriptionEs = string.Empty;
        [SerializeField] private string userTrackingUsageDescriptionFr = string.Empty;
        [SerializeField] private string userTrackingUsageDescriptionJa = string.Empty;
        [SerializeField] private string userTrackingUsageDescriptionKo = string.Empty;
        [SerializeField] private string userTrackingUsageDescriptionZhHans = string.Empty;
        [SerializeField] private string userTrackingUsageDescriptionZhHant = string.Empty;

        private const string SettingsFilePath = "ProjectSettings/AppLovinInternalSettings.json";

        public static AppLovinInternalSettings Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = CreateInstance<AppLovinInternalSettings>();

                var projectRootPath = Path.GetDirectoryName(Application.dataPath);
                var settingsFilePath = Path.Combine(projectRootPath, SettingsFilePath);
                if (!File.Exists(settingsFilePath))
                {
                    _instance.Save();
                    return _instance;
                }

                var settingsJson = File.ReadAllText(settingsFilePath);
                if (string.IsNullOrEmpty(settingsJson))
                {
                    _instance.Save();
                    return _instance;
                }

                JsonUtility.FromJsonOverwrite(settingsJson, _instance);
                return _instance;
            }
        }

        public void Save()
        {
            var settingsJson = JsonUtility.ToJson(_instance);
            try
            {
                var projectRootPath = Path.GetDirectoryName(Application.dataPath);
                var settingsFilePath = Path.Combine(projectRootPath, SettingsFilePath);
                File.WriteAllText(settingsFilePath, settingsJson);
            }
            catch (Exception exception)
            {
                MaxSdkLogger.UserError("Failed to save internal settings.");
                Console.WriteLine(exception);
            }
        }

        /// <summary>
        /// Whether or not AppLovin Consent Flow is enabled.
        /// </summary>
        public bool ConsentFlowEnabled
        {
            get { return consentFlowEnabled; }
            set
            {
                var previousValue = consentFlowEnabled;
                consentFlowEnabled = value;

                if (value)
                {
                    // If the value didn't change, we don't need to update anything.
                    if (previousValue) return;

                    UserTrackingUsageDescriptionEn = DefaultUserTrackingDescriptionEn;
                    UserTrackingUsageLocalizationEnabled = true;
                }
                else
                {
                    ConsentFlowPrivacyPolicyUrl = string.Empty;
                    ConsentFlowTermsOfServiceUrl = string.Empty;
                    UserTrackingUsageDescriptionEn = string.Empty;
                    UserTrackingUsageLocalizationEnabled = false;
                    OverrideDefaultUserTrackingUsageDescriptions = false;
                }
            }
        }

        /// <summary>
        /// A URL pointing to the Privacy Policy for the app to be shown when prompting the user for consent.
        /// </summary>
        public string ConsentFlowPrivacyPolicyUrl
        {
            get { return consentFlowPrivacyPolicyUrl; }
            set { consentFlowPrivacyPolicyUrl = value; }
        }

        /// <summary>
        /// An optional URL pointing to the Terms of Service for the app to be shown when prompting the user for consent. 
        /// </summary>
        public string ConsentFlowTermsOfServiceUrl
        {
            get { return consentFlowTermsOfServiceUrl; }
            set { consentFlowTermsOfServiceUrl = value; }
        }

        /// <summary>
        /// Whether or not to show the Terms and Privacy Policy alert in GDPR regions prior to presenting the CMP prompt.
        /// </summary>
        public bool ShouldShowTermsAndPrivacyPolicyAlertInGDPR
        {
            get { return shouldShowTermsAndPrivacyPolicyAlertInGDPR; }
            set { shouldShowTermsAndPrivacyPolicyAlertInGDPR = value; }
        }

        /// <summary>
        /// A User Tracking Usage Description in English to be shown to users when requesting permission to use data for tracking.
        /// For more information see <see href="https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription">Apple's documentation</see>.
        /// </summary>
        public string UserTrackingUsageDescriptionEn
        {
            get { return userTrackingUsageDescriptionEn; }
            set { userTrackingUsageDescriptionEn = value; }
        }

        /// <summary>
        /// An optional string to set debug user geography
        /// </summary>
        public MaxSdkBase.ConsentFlowUserGeography DebugUserGeography
        {
            get { return debugUserGeography; }
            set { debugUserGeography = value; }
        }

        public bool OverrideDefaultUserTrackingUsageDescriptions
        {
            get { return overrideDefaultUserTrackingUsageDescriptions; }
            set
            {
                var previousValue = overrideDefaultUserTrackingUsageDescriptions;
                overrideDefaultUserTrackingUsageDescriptions = value;

                if (!value)
                {
                    if (!previousValue) return;

                    UserTrackingUsageDescriptionEn = DefaultUserTrackingDescriptionEn;
                    UserTrackingUsageDescriptionDe = DefaultUserTrackingDescriptionDe;
                    UserTrackingUsageDescriptionEs = DefaultUserTrackingDescriptionEs;
                    UserTrackingUsageDescriptionFr = DefaultUserTrackingDescriptionFr;
                    UserTrackingUsageDescriptionJa = DefaultUserTrackingDescriptionJa;
                    UserTrackingUsageDescriptionKo = DefaultUserTrackingDescriptionKo;
                    UserTrackingUsageDescriptionZhHans = DefaultUserTrackingDescriptionZhHans;
                    UserTrackingUsageDescriptionZhHant = DefaultUserTrackingDescriptionZhHant;
                }
            }
        }

        /// <summary>
        /// Whether or not to localize User Tracking Usage Description.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public bool UserTrackingUsageLocalizationEnabled
        {
            get { return userTrackingUsageLocalizationEnabled; }
            set
            {
                var previousValue = userTrackingUsageLocalizationEnabled;
                userTrackingUsageLocalizationEnabled = value;

                if (value)
                {
                    // If the value didn't change, don't do anything
                    if (previousValue) return;

                    // Don't set the default values if they are being overriden.
                    if (OverrideDefaultUserTrackingUsageDescriptions) return;

                    UserTrackingUsageDescriptionDe = DefaultUserTrackingDescriptionDe;
                    UserTrackingUsageDescriptionEs = DefaultUserTrackingDescriptionEs;
                    UserTrackingUsageDescriptionFr = DefaultUserTrackingDescriptionFr;
                    UserTrackingUsageDescriptionJa = DefaultUserTrackingDescriptionJa;
                    UserTrackingUsageDescriptionKo = DefaultUserTrackingDescriptionKo;
                    UserTrackingUsageDescriptionZhHans = DefaultUserTrackingDescriptionZhHans;
                    UserTrackingUsageDescriptionZhHant = DefaultUserTrackingDescriptionZhHant;
                }
                else
                {
                    UserTrackingUsageDescriptionDe = string.Empty;
                    UserTrackingUsageDescriptionEs = string.Empty;
                    UserTrackingUsageDescriptionFr = string.Empty;
                    UserTrackingUsageDescriptionJa = string.Empty;
                    UserTrackingUsageDescriptionKo = string.Empty;
                    UserTrackingUsageDescriptionZhHans = string.Empty;
                    UserTrackingUsageDescriptionZhHant = string.Empty;
                }
            }
        }

        /// <summary>
        /// A User Tracking Usage Description in German to be shown to users when requesting permission to use data for tracking.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public string UserTrackingUsageDescriptionDe
        {
            get { return userTrackingUsageDescriptionDe; }
            set { userTrackingUsageDescriptionDe = value; }
        }

        /// <summary>
        /// A User Tracking Usage Description in Spanish to be shown to users when requesting permission to use data for tracking.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public string UserTrackingUsageDescriptionEs
        {
            get { return userTrackingUsageDescriptionEs; }
            set { userTrackingUsageDescriptionEs = value; }
        }

        /// <summary>
        /// A User Tracking Usage Description in French to be shown to users when requesting permission to use data for tracking.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public string UserTrackingUsageDescriptionFr
        {
            get { return userTrackingUsageDescriptionFr; }
            set { userTrackingUsageDescriptionFr = value; }
        }

        /// <summary>
        /// A User Tracking Usage Description in Japanese to be shown to users when requesting permission to use data for tracking.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public string UserTrackingUsageDescriptionJa
        {
            get { return userTrackingUsageDescriptionJa; }
            set { userTrackingUsageDescriptionJa = value; }
        }

        /// <summary>
        /// A User Tracking Usage Description in Korean to be shown to users when requesting permission to use data for tracking.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public string UserTrackingUsageDescriptionKo
        {
            get { return userTrackingUsageDescriptionKo; }
            set { userTrackingUsageDescriptionKo = value; }
        }

        /// <summary>
        /// A User Tracking Usage Description in Chinese (Simplified) to be shown to users when requesting permission to use data for tracking.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public string UserTrackingUsageDescriptionZhHans
        {
            get { return userTrackingUsageDescriptionZhHans; }
            set { userTrackingUsageDescriptionZhHans = value; }
        }

        /// <summary>
        /// A User Tracking Usage Description in Chinese (Traditional) to be shown to users when requesting permission to use data for tracking.
        /// For more information see Apple's documentation: https://developer.apple.com/documentation/bundleresources/information_property_list/nsusertrackingusagedescription
        /// </summary>
        public string UserTrackingUsageDescriptionZhHant
        {
            get { return userTrackingUsageDescriptionZhHant; }
            set { userTrackingUsageDescriptionZhHant = value; }
        }
    }
}
