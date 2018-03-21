﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultApplicationServices.cs" company="Microsoft Corporation">
//   Copyright (c) 2008, 2009, 2010 All Rights Reserved, Microsoft Corporation
//
//   This source is subject to the Microsoft Permissive License.
//   Please see the License.txt file for more information.
//   All other rights reserved.
//
//   THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY 
//   KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//   IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//   PARTICULAR PURPOSE.
// </copyright>
// <summary>
//   Wrapper for CTS application settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Microsoft.Exchange.Data.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;

    /// <summary>
    /// Wrapper for CTS application settings.
    /// </summary>
    internal class DefaultApplicationServices : IApplicationServices
    {
        /// <summary>
        /// A blank sub section.
        /// </summary>
        private static readonly IList<CtsConfigurationSetting> EmptySubSection = new List<CtsConfigurationSetting>();

        /// <summary>
        /// The lock used for thread safe syncronization.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// The configuration sub sections from the CTS application settings.
        /// </summary>
        private volatile Dictionary<string, IList<CtsConfigurationSetting>> configurationSubSections = new Dictionary<string, IList<CtsConfigurationSetting>>
                                                                {
                                                                    { string.Empty, new List<CtsConfigurationSetting>() }
                                                                };


        /// <summary>
        /// Gets the configuration subsection specified.
        /// </summary>
        /// <param name="subSectionName">Name of the subsection.</param>
        /// <returns>
        /// A list of <see cref="CtsConfigurationSetting"/>s for the specified section.
        /// </returns>
        public IList<CtsConfigurationSetting> GetConfiguration(string subSectionName)
        {
            IList<CtsConfigurationSetting> subSection;

            if (subSectionName == null)
            {
                subSectionName = string.Empty;
            }

            if (!this.configurationSubSections.TryGetValue(subSectionName, out subSection))
            {
                subSection = EmptySubSection;
            }

            return subSection;
        }

        /// <summary>
        /// Logs an error during configuration processing.
        /// </summary>
        public void LogConfigurationErrorEvent()
        {
        }
    }
}
