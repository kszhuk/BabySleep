﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BabySleep.Resx {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StatisticsResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StatisticsResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BabySleep.Resx.StatisticsResources", typeof(StatisticsResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Day sleeps count.
        /// </summary>
        internal static string DayCount {
            get {
                return ResourceManager.GetString("DayCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Day sleeps hours.
        /// </summary>
        internal static string DayHours {
            get {
                return ResourceManager.GetString("DayHours", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please, select time interval that not exceeds 30 days.
        /// </summary>
        internal static string ExceedDaysLimit {
            get {
                return ResourceManager.GetString("ExceedDaysLimit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Night sleep hours.
        /// </summary>
        internal static string NightHours {
            get {
                return ResourceManager.GetString("NightHours", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Total sleeps hours.
        /// </summary>
        internal static string TotalHours {
            get {
                return ResourceManager.GetString("TotalHours", resourceCulture);
            }
        }
    }
}
