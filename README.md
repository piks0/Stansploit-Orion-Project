# Stansploit Orion Project

**Stansploit Orion Project** is a Windows system optimization tool built specifically for gamers who want to squeeze the most performance out of their PC without digging through registry keys and Group Policy settings by hand.

## Features

- ⚡ **Power Plan** — Creates and applies a custom "Orion Gaming" power plan tuned for low latency and maximum throughput (CPU core parking disabled, PCIe max performance, USB selective suspend off, and more).
- 📦 **Installer** — One-click install of essential gaming and monitoring tools (MSI Afterburner, RTSS, DDU, Discord, Steam) via winget/chocolatey.
- 🧹 **Debloater** — Removes Windows bloatware and unnecessary background services/telemetry, and manages startup programs — with a "Select All Safe" preset for quick cleanup.
- 🧪 **Experimental** — Advanced, clearly-flagged tweaks (Nagle's algorithm, HAGS, TDR delay, fullscreen optimization overrides) for users who want to push further, with per-tweak confirmation.
- 🔄 **Safe by design** — Every change is snapshotted before it's applied, logged in detail, and reversible via a "Restore Defaults" option.

## Status
🚧 In active development. Currently building out tab-by-tab functional logic, starting with Power Plan.

## Requirements
- Windows 10/11
- Administrator privileges (required for registry/service/power plan changes)

## Disclaimer
This tool modifies system-level settings, services, and the registry. While all changes are logged and reversible, use at your own risk — especially features in the Experimental tab.
