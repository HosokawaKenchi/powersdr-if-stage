//=============================================================================
// KenwoodRig.cs
//=============================================================================
// Author: Chad Gatesman (W1CEG)
//=============================================================================
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//=============================================================================

using System.Threading;
using System;


namespace PowerSDR
{
	public class KenwoodRig : SerialRig
	{
		public enum Mode
		{
			LSB = 1,
			USB = 2,
			CW = 3,
			FM = 4,
			AM = 5,
			FSK = 6,
		}


		public KenwoodRig(RigHW hw,Console console)
			: base(hw,console)
		{
		}


		#region Defaults & Supported Functions

		public override int defaultBaudRate()
		{
			return 4800;
		}

		public override bool needsPollVFOB()
		{
			return false;
		}

		public override bool supportsIFFreq()
		{
			return false;
		}

		public override int getModeFromDSPMode(DSPMode dspMode)
		{
			switch (dspMode)
			{
				case DSPMode.LSB:
					return (int) Mode.LSB;
				case DSPMode.USB:
					return (int) Mode.USB;
				case DSPMode.CWL:
				case DSPMode.CWU:
					return (int) Mode.CW;
				case DSPMode.FMN:
					return (int) Mode.FM;
				case DSPMode.AM:
					return (int) Mode.AM;
				case DSPMode.DIGU:
				case DSPMode.DIGL:
					return (int) Mode.FSK;
				default:
					return (int) Mode.LSB;
			}
		}

		public override void setConsoleModeFromString(string mode)
		{
			switch (mode)
			{
				case "1":
					this.console.RX1DSPMode = DSPMode.LSB;
					break;
				case "2":
					this.console.RX1DSPMode = DSPMode.USB;
					break;
				case "3":
					this.console.RX1DSPMode = DSPMode.CWU;
					break;
				case "4":
					this.console.RX1DSPMode = DSPMode.FMN;
					break;
				case "5":
					this.console.RX1DSPMode = DSPMode.AM;
					break;
				case "6":
					this.console.RX1DSPMode = DSPMode.DIGL;
					break;
				case "7":
					this.console.RX1DSPMode = DSPMode.CWL;
					break;
				case "9":
					this.console.RX1DSPMode = DSPMode.DIGU;
					break;
				default:
					break;
			}
		}

		public override bool ritAppliedInIFCATCommand()
		{
			return true;
		}

		public override double minFreq()
		{
			return 0.03;
		}

		public override double maxFreq()
		{
			return 30.0;
		}

		#endregion Defaults & Supported Functions


		#region Get CAT Commands

		public override void getRigInformation()
		{
			this.doRigCATCommand("IF;");
		}

		public override void getVFOAFreq()
		{
			this.doRigCATCommand("FA;");
		}

		public override void getVFOBFreq()
		{
			this.doRigCATCommand("FB;");
		}

		public override void getIFFreq()
		{
		}

		#endregion Get CAT Commands

		#region Set CAT Commands

		public override void setVFOAFreq(double freq)
		{
			if (!this.connected)
				return;

			string frequency =
				freq.ToString("f6").Replace(Rig.separator,"").PadLeft(11,'0');

			// Only do this if our Frequency State has changed.
			// :TODO: Do we need to pay attention to the VFO state?
			if (frequency == this.VFOAFrequency)
				return;

			this.enqueueRigCATCommand("FA" + frequency + ';');

			// Set our Frequency State so we don't do this again.
			this.VFOAFrequency = frequency;
		}

		public override void setVFOAFreq(string freq)
		{
			if (!this.connected)
				return;

			// Only do this if our Frequency State has changed.
			if (freq == this.VFOAFrequency)
				return;

			this.doRigCATCommand("FA" + freq + ';',false,false);

			// Set our Frequency State so we don't do this again.
			this.VFOAFrequency = freq;
		}

		public override void setVFOBFreq(double freq)
		{
			if (!this.connected)
				return;

			string frequency =
				freq.ToString("f6").Replace(Rig.separator,"").PadLeft(11,'0');

			// Only do this if our Frequency State has changed.
			// :TODO: Do we need to pay attention to the VFO state?
			if (frequency == this.VFOBFrequency)
				return;

			this.enqueueRigCATCommand("FB" + frequency + ';');

			// Set our Frequency State so we don't do this again.
			this.VFOBFrequency = frequency;
		}

		public override void setVFOBFreq(string freq)
		{
			if (!this.connected)
				return;

			// Only do this if our Frequency State has changed.
			if (freq == this.VFOBFrequency)
				return;

			this.doRigCATCommand("FB" + freq + ';',false,false);

			// Set our Frequency State so we don't do this again.
			this.VFOBFrequency = freq;
		}

		public override void setVFOA()
		{
			this.doRigCATCommand("FN0;",false,false);
		}

		public override void setVFOB()
		{
			this.doRigCATCommand("FN1;",false,false);
		}

		public override void setMode(DSPMode mode)
		{
			int setMode = this.getModeFromDSPMode(mode);

			if (!this.connected || this.VFOAMode == setMode)
				return;

			this.VFOAMode = setMode;
			this.doRigCATCommand("MD" + setMode + ';',true,false);
		}

		public override void setSplit(bool splitOn)
		{
			if (!this.connected || this.Split == splitOn)
				return;

			if (splitOn)
			{
				if (this.VFOAMode != this.VFOBMode)
				{
					// Jump to VFO-B and change the mode to sync with VFO-A...
					this.doRigCATCommand("FN1;",true,false);
					Thread.Sleep(this.hw.RigTuningPollingInterval);
					this.doRigCATCommand("MD" + this.VFOAMode + ';',true,false);
					Thread.Sleep(this.hw.RigTuningPollingInterval);
					this.doRigCATCommand("FN0;",true,false);
					this.VFOBMode = this.VFOAMode;
				}
			}

			this.doRigCATCommand("SP" + ((splitOn) ? '1' : '0') + ';',true,false);
			this.Split = splitOn;
		}

		public override void clearRIT()
		{
			this.doRigCATCommand("RC;",false,false);
			this.RITOffset = 0;
		}

		public override void setRIT(bool rit)
		{
			this.doRigCATCommand("RT" + ((rit) ? '1' : '0') + ';',true,false);
		}

		public override void setRIT(int ritOffset)
		{
			if (!this.RITOffsetInitialized || ritOffset == this.RITOffset)
				return;

			// If offsets have opposite polarity or difference from 0 is less,
			// clear RIT, first.
			if (ritOffset == 0 || ritOffset < 0 && this.RITOffset > 0 ||
				ritOffset > 0 && this.RITOffset < 0 ||
				Math.Abs(ritOffset) < Math.Abs(this.RITOffset - ritOffset))
			{
				this.doRigCATCommand("RC;",true,false);
				this.RITOffset = 0;
				Thread.Sleep(this.hw.RigTuningPollingInterval);
			}

			string cmd = (ritOffset > this.RITOffset) ? "RU;" : "RD;";

			for (int count = Math.Abs(ritOffset - this.RITOffset) / 10; count > 0; count--)
			{
				this.doRigCATCommand(cmd,true,false);

				if (count > 1)
					Thread.Sleep(this.hw.RigTuningPollingInterval);
			}

			this.RITOffset = ritOffset;
		}

		#endregion Set CAT Commands
	}
}