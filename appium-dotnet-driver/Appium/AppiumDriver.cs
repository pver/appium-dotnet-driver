﻿//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//See the NOTICE file distributed with this work for additional
//information regarding copyright ownership.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using Appium.Interfaces.Generic.SearchContext;
using Newtonsoft.Json;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace OpenQA.Selenium.Appium
{
    /// <summary>
    /// Provides a way to access Appium to run your tests by creating a AppiumDriver instance
    /// </summary>
    /// <remarks>
    /// When the WebDriver object has been instantiated the browser will load. The test can then navigate to the URL under test and 
    /// start your test.
    /// </remarks>
    /// <example>
    /// <code>
    /// [TestFixture]
    /// public class Testing
    /// {
    ///     private IWebDriver driver;
    ///     <para></para>
    ///     [SetUp]
    ///     public void SetUp()
    ///     {
    ///         driver = new AppiumDriver();
    ///     }
    ///     <para></para>
    ///     [Test]
    ///     public void TestGoogle()
    ///     {
    ///         driver.Navigate().GoToUrl("http://www.google.co.uk");
    ///         /*
    ///         *   Rest of the test
    ///         */
    ///     }
    ///     <para></para>
    ///     [TearDown]
    ///     public void TearDown()
    ///     {
    ///         driver.Quit();
    ///         driver.Dispose();
    ///     } 
    /// }
    /// </code>
    /// </example>
	public abstract class AppiumDriver<W> : RemoteWebDriver, ITouchShortcuts, IFindByAccessibilityId<W>, IDeviceActionShortcuts, IInteractsWithFiles,
        IInteractsWithApps, IPerformsTouchActions, IRotatable, IContextAware, IGenericSearchContext<W>, IGenericFindsByClassName<W>,
        IGenericFindsById<W>, IGenericFindsByCssSelector<W>, IGenericFindsByLinkText<W>, IGenericFindsByName<W>,
	IGenericFindsByPartialLinkText<W>, IGenericFindsByTagName<W>, IGenericFindsByXPath<W>, IScrollsTo<W> where W : IWebElement
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the RemoteWebDriver class
        /// </summary>
        /// <param name="commandExecutor">An <see cref="ICommandExecutor"/> object which executes commands for the driver.</param>
        /// <param name="desiredCapabilities">An <see cref="ICapabilities"/> object containing the desired capabilities of the browser.</param>
        private AppiumDriver(ICommandExecutor commandExecutor, ICapabilities desiredCapabilities)
            : base(commandExecutor, desiredCapabilities)
        {
            _AddAppiumCommands(commandExecutor.CommandInfoRepository);
        }



        /// <summary>
        /// Initializes a new instance of the AppiumDriver class. This constructor defaults proxy to http://127.0.0.1:4723/wd/hub
        /// </summary>
        /// <param name="desiredCapabilities">An <see cref="ICapabilities"/> object containing the desired capabilities of the browser.</param>
        public AppiumDriver(ICapabilities desiredCapabilities)
            : this(new Uri("http://127.0.0.1:4723/wd/hub"), desiredCapabilities)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AppiumDriver class
        /// </summary>
        /// <param name="remoteAddress">URI containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4723/wd/hub).</param>
        /// <param name="desiredCapabilities">An <see cref="ICapabilities"/> object containing the desired capabilities of the browser.</param>
        public AppiumDriver(Uri remoteAddress, ICapabilities desiredCapabilities)
            : this(remoteAddress, desiredCapabilities, RemoteWebDriver.DefaultCommandTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AppiumDriver class using the specified remote address, desired capabilities, and command timeout.
        /// </summary>
        /// <param name="remoteAddress">URI containing the address of the WebDriver remote server (e.g. http://127.0.0.1:4723/wd/hub).</param>
        /// <param name="desiredCapabilities">An <see cref="ICapabilities"/> object containing the desired capabilities of the browser.</param>
        /// <param name="commandTimeout">The maximum amount of time to wait for each command.</param>
        public AppiumDriver(Uri remoteAddress, ICapabilities desiredCapabilities, TimeSpan commandTimeout)
            : this(CommandExecutorFactory.GetHttpCommandExecutor(remoteAddress, commandTimeout), desiredCapabilities)
        {
        }
        #endregion Constructors

        #region Generic FindMethods
        /// <summary>
        /// Finds the first element in the page that matches the OpenQA.Selenium.By object 
        /// </summary>
        /// <param name="by">Mechanism to find element</param>
        /// <returns>first element found</returns>
        public new W FindElement(By by)
        {
            return (W)base.FindElement(by);
        }

        /// <summary>
        /// Find the elements on the page by using the <see cref="T:OpenQA.Selenium.By"/> object and returns a ReadonlyCollection of the Elements on the page 
        /// </summary>
        /// <param name="by">Mechanism to find element</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElements(By by)
        {
            return CollectionConverterUnility.
                ConvertToExtendedWebElementCollection<W>(base.FindElements(by));
        }

        /// <summary>
        /// Finds the first element in the page that matches the class name supplied
        /// </summary>
        /// <param name="className">CSS class name on the element</param>
        /// <returns>first element found</returns>
        public new W FindElementByClassName(string className)
        {
            return (W)base.FindElementByClassName(className);
        }

        /// <summary>
        /// Finds a list of elements that match the class name supplied
        /// </summary>
        /// <param name="className">CSS class name on the element</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsByClassName(string className)
        {
            return CollectionConverterUnility.
                ConvertToExtendedWebElementCollection<W>(base.FindElementsByClassName(className));
        }

        /// <summary>
        /// Finds the first element in the page that matches the ID supplied
        /// </summary>
        /// <param name="id">ID of the element</param>
        /// <returns>First element found</returns>
        public new W FindElementById(string id)
        {
            return (W)base.FindElementById(id);
        }

        /// <summary>
        /// Finds a list of elements that match the ID supplied
        /// </summary>
        /// <param name="id">ID of the element</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsById(string id)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElementsById(id));
        }

        /// <summary>
        /// Finds the first element matching the specified CSS selector
        /// </summary>
        /// <param name="cssSelector">The CSS selector to match</param>
        /// <returns>First element found</returns>
        public new W FindElementByCssSelector(string cssSelector)
        {
            return (W)base.FindElementByCssSelector(cssSelector);
        }

        /// <summary>
        /// Finds a list of elements that match the CSS selector
        /// </summary>
        /// <param name="cssSelector">The CSS selector to match</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsByCssSelector(string cssSelector)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElementsByCssSelector(cssSelector));
        }

        /// <summary>
        /// Finds the first of elements that match the link text supplied
        /// </summary>
        /// <param name="linkText">Link text of element</param>
        /// <returns>First element found</returns>
        public new W FindElementByLinkText(string linkText)
        {
            return (W)base.FindElementByLinkText(linkText);
        }

        /// <summary>
        /// Finds a list of elements that match the link text supplied
        /// </summary>
        /// <param name="linkText">Link text of element</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsByLinkText(string linkText)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElementsByLinkText(linkText));
        }

        /// <summary>
        /// Finds the first of elements that match the name supplied
        /// </summary>
        /// <param name="name">Name of the element on the page</param>
        /// <returns>First element found</returns>
        public new W FindElementByName(string name)
        {
            return (W)base.FindElementByName(name);
        }

        /// <summary>
        /// Finds a list of elements that match the name supplied
        /// </summary>
        /// <param name="name">Name of the element on the page</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsByName(string name)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElementsByName(name));
        }

        /// <summary>
        /// Finds the first of elements that match the part of the link text supplied
        /// </summary>
        /// <param name="partialLinkText">Part of the link text</param>
        /// <returns>First element found</returns>
        public new W FindElementByPartialLinkText(string partialLinkText)
        {
            return (W)base.FindElementByPartialLinkText(partialLinkText);
        }

        /// <summary>
        /// Finds a list of elements that match the part of the link text supplied
        /// </summary>
        /// <param name="partialLinkText">Part of the link text</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsByPartialLinkText(string partialLinkText)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElementsByPartialLinkText(partialLinkText));
        }

        /// <summary>
        /// Finds the first of elements that match the DOM Tag supplied
        /// </summary>
        /// <param name="tagName">DOM tag name of the element being searched</param>
        /// <returns>First element found</returns>
        public new W FindElementByTagName(string tagName)
        {
            return (W)base.FindElementByTagName(tagName);
        }

        /// <summary>
        /// Finds a list of elements that match the DOM Tag supplied
        /// </summary>
        /// <param name="tagName">DOM tag name of the element being searched</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsByTagName(string tagName)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElementsByTagName(tagName));
        }

        /// <summary>
        /// Finds the first of elements that match the XPath supplied
        /// </summary>
        /// <param name="xpath">xpath to the element</param>
        /// <returns>First element found</returns>
        public new W FindElementByXPath(string xpath)
        {
            return (W)base.FindElementByXPath(xpath);
        }

        /// <summary>
        /// Finds a list of elements that match the XPath supplied
        /// </summary>
        /// <param name="xpath">xpath to the element</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public new ReadOnlyCollection<W> FindElementsByXPath(string xpath)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElementsByXPath(xpath));
        }

        #region IFindByAccessibilityId Members

        /// <summary>
        /// Finds the first element that match the accessibility id supplied
        /// </summary>
        /// <param name="selector">accessibility id</param>
        /// <returns>First element found</returns>
        public W FindElementByAccessibilityId(string selector)
        {
            return (W)this.FindElement("accessibility id", selector);
        }

        /// <summary>
        /// Finds a list of elements that match the accessibillity id supplied
        /// </summary>
        /// <param name="selector">accessibility id</param>
        /// <returns>ReadOnlyCollection of elements found</returns>
        public ReadOnlyCollection<W> FindElementsByAccessibilityId(string selector)
        {
            return CollectionConverterUnility.
                            ConvertToExtendedWebElementCollection<W>(base.FindElements("accessibility id", selector));
        }

        #endregion IFindByAccessibilityId Members

        #endregion

        #region Public Methods


        #region MJsonMethod Members
        /// <summary>
        /// Locks the device.
        /// </summary>
        /// <param name="seconds">The number of seconds during which the device need to be locked for.</param>
        public void LockDevice(int seconds)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("seconds", seconds);
            this.Execute(AppiumDriverCommand.LockDevice, parameters);
        }

        /// <summary>
        /// Triggers Device Key Event.
        /// </summary>
        /// <param name="keyCode">an integer keycode number corresponding to a java.awt.event.KeyEvent.</param>
        public void KeyEvent(int keyCode)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("keycode", keyCode);
            this.Execute(AppiumDriverCommand.KeyEvent, parameters);
        }

        /// <summary>
        /// Rotates Device.
        /// </summary>
        /// <param name="opts">rotations options like the following:
        /// new Dictionary<string, int> {{"x", 114}, {"y", 198}, {"duration", 5}, 
        /// {"radius", 3}, {"rotation", 220}, {"touchCount", 2}}
        /// </param>
        public void Rotate(Dictionary<string, int> opts)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (KeyValuePair<string, int> opt in opts)
            {
                parameters.Add(opt.Key, opt.Value);
            }
            this.Execute(AppiumDriverCommand.Rotate, parameters);
        }

        /// <summary>
        /// Installs an App.
        /// </summary>
        /// <param name="appPath">a string containing the file path or url of the app.</param>
        public void InstallApp(string appPath)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("appPath", appPath);
            this.Execute(AppiumDriverCommand.InstallApp, parameters);
        }

        /// <summary>
        /// Removes an App.
        /// </summary>
        /// <param name="appPath">a string containing the id of the app.</param>
        public void RemoveApp(string appId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("appId", appId);
            this.Execute(AppiumDriverCommand.RemoveApp, parameters);
        }

        /// <summary>
        /// Checks If an App Is Installed.
        /// </summary>
        /// <param name="appPath">a string containing the bundle id.</param>
        /// <return>a bol indicating if the app is installed.</return>
        public bool IsAppInstalled(string bundleId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("bundleId", bundleId);
            var commandResponse = this.Execute(AppiumDriverCommand.IsAppInstalled, parameters);
            Console.Out.WriteLine(commandResponse.Value);
            return Convert.ToBoolean(commandResponse.Value.ToString());
        }

        /// <summary>
        /// Pulls a File.
        /// </summary>
        /// <param name="pathOnDevice">path on device to pull</param>
        public byte[] PullFile(string pathOnDevice)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("path", pathOnDevice);
            var commandResponse = this.Execute(AppiumDriverCommand.PullFile, parameters);
            return Convert.FromBase64String(commandResponse.Value.ToString());
        }

        /// <summary>
        /// Pulls a Folder
        /// </summary>
        /// <param name="remotePath">remote path to the folder to return</param>
        /// <returns>a base64 encoded string representing a zip file of the contents of the folder</returns>
        public byte[] PullFolder(string remotePath)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("path", remotePath);
            var commandResponse = this.Execute(AppiumDriverCommand.PullFolder, parameters);
            return Convert.FromBase64String(commandResponse.Value.ToString());
        }

        /// <summary>
        /// Launches the current app.
        /// </summary>
        public void LaunchApp()
        {
            this.Execute(AppiumDriverCommand.LaunchApp, null);
        }

        /// <summary>
        /// Closes the current app.
        /// </summary>
        public void CloseApp()
        {
            this.Execute(AppiumDriverCommand.CloseApp, null);
        }

        /// <summary>
        /// Resets the current app.
        /// </summary>
        public void ResetApp()
        {
            this.Execute(AppiumDriverCommand.ResetApp, null);
        }

        /// <summary>
        /// Backgrounds the current app for the given number of seconds.
        /// </summary>
        /// <param name="seconds">a string containing the number of seconds.</param>
        public void BackgroundApp(int seconds)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("seconds", seconds);
            this.Execute(AppiumDriverCommand.BackgroundApp, parameters);
        }


        /// <summary>
        /// Gets the App Strings.
        /// </summary>
        public string GetAppStrings(string language = null)
        {
            Dictionary<string, object> parameters = null;
            if (null != language)
            {
                parameters = new Dictionary<string, object> { { "language", language } };
            }

            var commandResponse = this.Execute(AppiumDriverCommand.GetAppStrings, parameters);
            return commandResponse.Value as string;
        }

        /// <summary>
        /// Hide the keyboard
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="key"></param>
        protected void HideKeyboard(string strategy = null, string key = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (strategy != null) parameters.Add("strategy", strategy);
            if (key != null) parameters.Add("keyName", key);
            this.Execute(AppiumDriverCommand.HideKeyboard, parameters);
        }

        /// <summary>
        /// Hides the device keyboard.
        /// </summary>
        /// <param name="keyName">The button pressed by the mobile driver to attempt hiding the keyboard.</param>
        public void HideKeyboard()
        {
            this.HideKeyboard(null, null);
        }

        /// <sumary>
        /// GPS Location
        /// </summary>
        public Location Location
        {
            get
            {
                var commandResponse = this.Execute(AppiumDriverCommand.GetLocation, null);
                return JsonConvert.DeserializeObject<Location>((String)commandResponse.Value);
            }
            set
            {
                var location = value ?? new Location();
                Execute(AppiumDriverCommand.SetLocation, location.ToDictionary());
            }
        }

        #endregion MJsonMethod Members

        #region Context
        public string Context
        {
            get
            {
                var commandResponse = this.Execute(AppiumDriverCommand.GetContext, null);
                return commandResponse.Value as string;
            }
            set
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("name", value);
                this.Execute(AppiumDriverCommand.SetContext, parameters);
            }
        }

        public ReadOnlyCollection<string> Contexts
        {
            get
            {
                var commandResponse = this.Execute(AppiumDriverCommand.Contexts, null);
                var contexts = new List<string>();
                var objects = commandResponse.Value as object[];

                if (null != objects && 0 < objects.Length)
                {
                    contexts.AddRange(objects.Select(o => o.ToString()));
                }

                return contexts.AsReadOnly();
            }
        }
        #endregion Context

        #region Orientation
        /// <summary>
        /// Sets/Gets the Orientation
        /// </summary>
        public ScreenOrientation Orientation
        {
            get
            {
                var commandResponse = this.Execute(AppiumDriverCommand.GetOrientation, null);
                return (commandResponse.Value as string).ConvertToScreenOrientation();
            }
            set
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("orientation", value.JSONWireProtocolString());
                this.Execute(AppiumDriverCommand.SetOrientation, parameters);
            }
        }
        #endregion Orientation

        #region Input Method (IME)
        /// <summary>
        /// Get a list of IME engines available on the device
        /// </summary>
        /// <returns>List of available </returns>
        public List<string> GetIMEAvailableEngines()
        {
            var retVal = new List<string>();
            var commandResponse = this.Execute(AppiumDriverCommand.GetAvailableEngines, null);
            var objectArr = commandResponse.Value as object[];
            if (null != objectArr)
            {
                retVal.AddRange(objectArr.Select(val => val.ToString()));
            }
            return retVal;
        }

        /// <summary>
        /// Get the currently active IME Engine on the device
        /// </summary>
        /// <returns>Active IME Engine</returns>
        public string GetIMEActiveEngine()
        {
            var commandResponse = this.Execute(AppiumDriverCommand.GetActiveEngine, null);
            return commandResponse.Value as string;
        }

        /// <summary>
        /// Is the IME active on the device (NOTE: on Android, this is always true)
        /// </summary>
        /// <returns>true if IME is active, false otherwise</returns>
        public bool IsIMEActive()
        {
            var commandResponse = this.Execute(AppiumDriverCommand.IsIMEActive, null);
            return (bool)(commandResponse.Value);
        }

        /// <summary>
        /// Activate the given IME on Device
        /// </summary>
        /// <param name="imeEngine">IME to activate</param>
        public void ActivateIMEEngine(string imeEngine)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("engine", imeEngine);
            this.Execute(AppiumDriverCommand.ActivateEngine, parameters);
        }

        /// <summary>
        /// Deactivate the currently Active IME Engine on device
        /// </summary>
        public void DeactiveIMEEngine()
        {
            this.Execute(AppiumDriverCommand.DeactivateEngine, null);
        }
        #endregion Input Method (IME)

        #region Multi Actions

        /// <summary>
        /// Perform the multi action
        /// </summary>
        /// <param name="multiAction">multi action to perform</param>
        public void PerformMultiAction(MultiAction multiAction)
        {
            if (null == multiAction)
            {
                return; // do nothing
            }

            var parameters = multiAction.GetParameters();
            this.Execute(AppiumDriverCommand.MultiActionV2Perform, parameters);
        }

        /// <summary>
        /// Perform the touch action
        /// </summary>
        /// <param name="touchAction">touch action to perform</param>
        public void PerformTouchAction(TouchAction touchAction)
        {
            if (null == touchAction)
            {
                return; // do nothing
            }

            var parameters = new Dictionary<string, object>();
            parameters.Add("actions", touchAction.GetParameters());
            this.Execute(AppiumDriverCommand.TouchActionV2Perform, parameters);
        }

        #endregion Multi Actions

        #region Settings

        /// <summary>
        /// Get appium settings currently set for the session
        /// See: https://github.com/appium/appium/blob/master/docs/en/advanced-concepts/settings.md
        /// </summary>
        public Dictionary<String, Object> GetSettings()
        {
            var commandResponse = this.Execute(AppiumDriverCommand.GetSettings, null);
            return JsonConvert.DeserializeObject<Dictionary<String, Object>>((String)commandResponse.Value);
        }

        /// <summary>
        /// Update an appium Setting, on the session
        /// </summary>
        protected void UpdateSetting(String setting, Object value)
        {
            var parameters = new Dictionary<string, object>();
            var settings = new Dictionary<string, object>();
            settings.Add(setting, value);
            parameters.Add("settings", settings);
            this.Execute(AppiumDriverCommand.UpdateSettings, parameters);
        }
        #endregion Settings

        #region tap, swipe, pinch, zoom

        /// <summary>
        /// Creates a tap on an element for a given time
        /// </summary>
		private ITouchAction CreateTap(IWebElement element, int duration)
        {
            TouchAction tap = new TouchAction(this);
            return tap.Press(element).Wait(duration).Release();
        }

        /// <summary>
        /// Creates a tap on x-y-coordinates for a given time
        /// </summary>
        private ITouchAction CreateTap(int x, int y, int duration)
        {
            TouchAction tap = new TouchAction(this);
            return tap.Press(x, y).Wait(duration).Release();
        }

        /// <summary>
        /// Convenience method for tapping the center of an element on the screen
        /// </summary>
        /// <param name="fingers">number of fingers/appendages to tap with</param>
        /// <param name="element">element to tap</param>
        /// <param name="duration">how long between pressing down, and lifting fingers/appendages</param>
        public void Tap(int fingers, IWebElement element, int duration)
        {
            MultiAction multiTouch = new MultiAction(this);

            for (int i = 0; i < fingers; i++)
            {
                multiTouch.Add(CreateTap(element, duration));
            }

            multiTouch.Perform();
        }

        /// <summary>
        /// Convenience method for tapping a position on the screen
        /// </summary>
        /// <param name="fingers">number of fingers/appendages to tap with</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="duration">how long between pressing down, and lifting fingers/appendages</param>
        public void Tap(int fingers, int x, int y, int duration)
        {
            MultiAction multiTouch = new MultiAction(this);

            for (int i = 0; i < fingers; i++)
            {
                multiTouch.Add(CreateTap(x, y, duration));
            }

            multiTouch.Perform();
        }

        /// <summary>
        /// Convenience method for swiping across the screen
        /// </summary>
        /// <param name="startx">starting x coordinate</param>
        /// <param name="starty">starting y coordinate</param>
        /// <param name="endx">ending x coordinate</param>
        /// <param name="endy">ending y coordinate</param>
        /// <param name="duration">amount of time in milliseconds for the entire swipe action to take</param>
        public void Swipe(int startx, int starty, int endx, int endy, int duration)
        {
            TouchAction touchAction = new TouchAction(this);

            // appium converts Press-wait-MoveTo-Release to a swipe action
            touchAction.Press(startx, starty).Wait(duration)
                    .MoveTo(endx, endy).Release();

            touchAction.Perform();
        }

        /// <summary>
        /// Convenience method for pinching an element on the screen.
        /// "pinching" refers to the action of two appendages Pressing the screen and sliding towards each other.
        /// NOTE:
        /// driver convenience method places the initial touches around the element, if driver would happen to place one of them
        /// off the screen, appium with return an outOfBounds error. In driver case, revert to using the MultiAction api
        /// instead of driver method.
        /// </summary>
        /// <param name="el">The element to pinch</param>
		public void Pinch(IWebElement el)
        {
            MultiAction multiTouch = new MultiAction(this);

            Size dimensions = el.Size;
            Point upperLeft = el.Location;
            Point center = new Point(upperLeft.X + dimensions.Width / 2, upperLeft.Y + dimensions.Height / 2);
            int yOffset = center.Y - upperLeft.Y;

            ITouchAction action0 = new TouchAction(this).Press(el, center.X, center.Y - yOffset).MoveTo(el).Release();
            ITouchAction action1 = new TouchAction(this).Press(el, center.X, center.Y + yOffset).MoveTo(el).Release();

            multiTouch.Add(action0).Add(action1);

            multiTouch.Perform();
        }

        /// <summary>
        /// Convenience method for pinching an element on the screen.
        /// "pinching" refers to the action of two appendages Pressing the screen and sliding towards each other.
        /// NOTE:
        /// driver convenience method places the initial touches around the element at a distance, if driver would happen to place
        /// one of them off the screen, appium will return an outOfBounds error. In driver case, revert to using the
        /// MultiAction api instead of driver method.
        /// </summary>
        /// <param name="x">x coordinate to terminate the pinch on</param>
        /// <param name="y">y coordinate to terminate the pinch on></param>
        public void Pinch(int x, int y)
        {
            MultiAction multiTouch = new MultiAction(this);

            int scrHeight = Manage().Window.Size.Height;
            int yOffset = 100;

            if (y - 100 < 0)
            {
                yOffset = y;
            }
            else if (y + 100 > scrHeight)
            {
                yOffset = scrHeight - y;
            }

            ITouchAction action0 = new TouchAction(this).Press(x, y - yOffset).MoveTo(x, y).Release();
            ITouchAction action1 = new TouchAction(this).Press(x, y + yOffset).MoveTo(x, y).Release();

            multiTouch.Add(action0).Add(action1);

            multiTouch.Perform();
        }

        /// <summary>
        /// Convenience method for "zooming in" on an element on the screen.
        /// "zooming in" refers to the action of two appendages Pressing the screen and sliding away from each other.
        /// NOTE:
        /// driver convenience method slides touches away from the element, if driver would happen to place one of them
        /// off the screen, appium will return an outOfBounds error. In driver case, revert to using the MultiAction api
        /// instead of driver method.
        /// <param name="x">x coordinate to terminate the zoom on</param>
        /// <param name="y">y coordinate to terminate the zoom on></param>
        /// </summary>
        public void Zoom(int x, int y)
        {
            MultiAction multiTouch = new MultiAction(this);

            int scrHeight = Manage().Window.Size.Height;
            int yOffset = 100;

            if (y - 100 < 0)
            {
                yOffset = y;
            }
            else if (y + 100 > scrHeight)
            {
                yOffset = scrHeight - y;
            }

            ITouchAction action0 = new TouchAction(this).Press(x, y).MoveTo(x, y - yOffset).Release();
            ITouchAction action1 = new TouchAction(this).Press(x, y).MoveTo(x, y + yOffset).Release();

            multiTouch.Add(action0).Add(action1);

            multiTouch.Perform();
        }

        /// <summary>
        /// Convenience method for "zooming in" on an element on the screen.
        /// "zooming in" refers to the action of two appendages Pressing the screen and sliding away from each other.
        /// NOTE:
        /// driver convenience method slides touches away from the element, if driver would happen to place one of them
        /// off the screen, appium will return an outOfBounds error. In driver case, revert to using the MultiAction api
        /// instead of driver method.
        /// <param name="el">The element to pinch</param>
        /// </summary>
		public void Zoom(IWebElement el)
        {
            MultiAction multiTouch = new MultiAction(this);

            Size dimensions = el.Size;
            Point upperLeft = el.Location;
            Point center = new Point(upperLeft.X + dimensions.Width / 2, upperLeft.Y + dimensions.Height / 2);
            int yOffset = center.Y - upperLeft.Y;

            ITouchAction action0 = new TouchAction(this).Press(el).MoveTo(el, center.X, center.Y - yOffset).Release();
            ITouchAction action1 = new TouchAction(this).Press(el).MoveTo(el, center.X, center.Y + yOffset).Release();

            multiTouch.Add(action0).Add(action1);

            multiTouch.Perform();
        }

        #endregion
        
        #endregion Public Methods

        #region Internal Methods
        /// <summary>
        /// Executes commands with the driver 
        /// </summary>
        /// <param name="driverCommandToExecute">Command that needs executing</param>
        /// <param name="parameters">Parameters needed for the command</param>
        /// <returns>WebDriver Response</returns>
        internal Response InternalExecute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            return this.Execute(driverCommandToExecute, parameters);
        }
        #endregion Internal Methods

        #region Support methods
        /// <summary>
        /// In derived classes, the <see cref="PrepareEnvironment"/> method prepares the environment for test execution.
        /// </summary>
        protected virtual void PrepareEnvironment()
        {
            // Does nothing, but provides a hook for subclasses to do "stuff"
        }

        /// <summary>
        /// Find the element in the response
        /// </summary>
        /// <param name="response">Response from the browser</param>
        /// <returns>Element from the page</returns>
        internal IWebElement GetElementFromResponse(Response response)
        {
            if (response == null)
            {
                throw new NoSuchElementException();
            }

            RemoteWebElement element = null;
            Dictionary<string, object> elementDictionary = response.Value as Dictionary<string, object>;
            if (elementDictionary != null)
            {
                string id = (string)elementDictionary["ELEMENT"];
                element = this.CreateElement(id);
            }

            return element;
        }

        internal static DesiredCapabilities SetPlatformToCapabilities(DesiredCapabilities dc, string desiredPlatform)
        {
            dc.SetCapability(MobileCapabilityType.PlatformName, desiredPlatform);
            return dc;
        }

        /// <summary>
        /// Finds the elements that are in the response
        /// </summary>
        /// <param name="response">Response from the browser</param>
        /// <returns>Collection of elements</returns>
        internal ReadOnlyCollection<IWebElement> GetElementsFromResponse(Response response)
        {
            List<IWebElement> toReturn = new List<IWebElement>();
            object[] elements = response.Value as object[];
            foreach (object elementObject in elements)
            {
                Dictionary<string, object> elementDictionary = elementObject as Dictionary<string, object>;
                if (elementDictionary != null)
                {
                    string id = (string)elementDictionary["ELEMENT"];
                    RemoteWebElement element = this.CreateElement(id);
                    toReturn.Add(element);
                }
            }

            return toReturn.AsReadOnly();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add the set of appium commands
        /// </summary>
        private static void _AddAppiumCommands(CommandInfoRepository repo)
        {
            var commandList = new List<_Commands>()
            {
                #region Context Commands
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.Contexts, "/session/{sessionId}/contexts" ),
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.GetContext, "/session/{sessionId}/context" ),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.SetContext, "/session/{sessionId}/context" ),
                #endregion Context Commands
                #region Appium Commands
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.ShakeDevice, "/session/{sessionId}/appium/device/shake"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.LockDevice, "/session/{sessionId}/appium/device/lock"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.IsLocked, "/session/{sessionId}/appium/device/is_locked"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.ToggleAirplaneMode, "/session/{sessionId}/appium/device/toggle_airplane_mode"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.KeyEvent, "/session/{sessionId}/appium/device/keyevent"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.Rotate, "/session/{sessionId}/appium/device/rotate"),
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.GetCurrentActivity, "/session/{sessionId}/appium/device/current_activity"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.InstallApp, "/session/{sessionId}/appium/device/install_app"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.RemoveApp, "/session/{sessionId}/appium/device/remove_app"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.IsAppInstalled, "/session/{sessionId}/appium/device/app_installed"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.PushFile, "/session/{sessionId}/appium/device/push_file"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.PullFile, "/session/{sessionId}/appium/device/pull_file"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.PullFolder, "/session/{sessionId}/appium/device/pull_folder"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.ToggleWiFi, "/session/{sessionId}/appium/device/toggle_wifi"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.ToggleLocationServices, "/session/{sessionId}/appium/device/toggle_location_services"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.LaunchApp, "/session/{sessionId}/appium/app/launch"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.CloseApp, "/session/{sessionId}/appium/app/close"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.ResetApp, "/session/{sessionId}/appium/app/reset"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.BackgroundApp, "/session/{sessionId}/appium/app/background"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.EndTestCoverage, "/session/{sessionId}/appium/app/end_test_coverage"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.GetAppStrings, "/session/{sessionId}/appium/app/strings"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.SetImmediateValue, "/session/{sessionId}/appium/element/{id}/value"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.HideKeyboard, "/session/{sessionId}/appium/device/hide_keyboard"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.OpenNotifications, "/session/{sessionId}/appium/device/open_notifications"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.StartActivity, "/session/{sessionId}/appium/device/start_activity"),
                new _Commands(CommandInfo.GetCommand,  AppiumDriverCommand.GetSettings, "/session/{sessionId}/appium/settings"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.UpdateSettings, "/session/{sessionId}/appium/settings"),
                #endregion Appium Commands
                #region Touch Commands
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.MultiActionV2Perform, "/session/{sessionId}/touch/multi/perform"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.TouchActionV2Perform, "/session/{sessionId}/touch/perform"),
                #endregion Touch Commands

                #region JSON Wire Protocol Commands
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.GetOrientation, "/session/{sessionId}/orientation"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.SetOrientation, "/session/{sessionId}/orientation"),
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.GetConnectionType, "/session/{sessionId}/network_connection"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.SetConnectionType, "/session/{sessionId}/network_connection"),
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.GetLocation, "/session/{sessionId}/location"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.SetLocation, "/session/{sessionId}/location"),

                #region Input Method (IME)
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.GetAvailableEngines, "/session/{sessionId}/ime/available_engines"),
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.GetActiveEngine, "/session/{sessionId}/ime/active_engine"),
                new _Commands(CommandInfo.GetCommand, AppiumDriverCommand.IsIMEActive, "/session/{sessionId}/ime/activated"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.ActivateEngine, "/session/{sessionId}/ime/activate"),
                new _Commands(CommandInfo.PostCommand, AppiumDriverCommand.DeactivateEngine, "/session/{sessionId}/ime/deactivate"),
                #endregion Input Method (IME) 

                #endregion JSON Wire Protocol Commands
                
            };

            foreach (_Commands entry in commandList)
            {
                var commandInfo = new CommandInfo(entry.CommandType, entry.ApiEndpoint);
                repo.TryAddCommand(entry.Command, commandInfo);
            }
        }

        #endregion Private Methods

        #region Private Class
        /// <summary>
        /// Container class for the command tuple
        /// </summary>
        private class _Commands
        {
            /// <summary>
            /// command type 
            /// </summary>
            internal readonly string CommandType;

            /// <summary>
            /// Command 
            /// </summary>
            internal readonly string Command;

            /// <summary>
            /// API Endpoint
            /// </summary>
            internal readonly string ApiEndpoint;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="commandType">type of command (get/post/delete)</param>
            /// <param name="command">Command</param>
            /// <param name="apiEndpoint">api endpoint</param>
            internal _Commands(string commandType, string command, string apiEndpoint)
            {
                CommandType = commandType;
                Command = command;
                ApiEndpoint = apiEndpoint;
            }
        }
        #endregion Private Class

        #region abstract scrolling methods

        public abstract W ScrollTo(string text);
        public abstract W ScrollToExact(string text);

        #endregion
    }
}
