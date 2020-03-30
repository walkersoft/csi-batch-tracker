﻿using CSI.BatchTracker.ViewModels.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSI.BatchTracker.Tests.ViewModels.Commands
{
    [TestFixture]
    class AutoBackupToggleCommandTest
    {
        ICommand command;

        [SetUp]
        public void SetUp()
        {
            command = new AutoBackupToggleCommand();
        }

        [Test]
        public void CommandCanAlwaysExecute()
        {
            Assert.True(command.CanExecute(null));
        }

        [Test]
        public void CommandWillNotTossException()
        {
            Assert.DoesNotThrow(() => command.Execute(null));
        }
    }
}
