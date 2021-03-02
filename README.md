# CSI Batch Tracker

## About
A small C#/WPF Windows desktop application to track and manage paint inventory.

## Why??
Cedar Siding, Inc. needs to track stain usage for a number of production and quality 
related reasons. From a production standpoint we need to have trackability on
how and when totes and barrels of coating are being used. From a quality control
standpoint this is useful for years later in the event of a coating failure in
the field. Batch/LOT information can be captured and recorded for later use
in warranty claims with the coating manufacturer.

## Requirements
- Windows 7 SP1 or greater
- .NET Framework 4.6.1 or greater

## Features

### Inventory & Personnel Management
- Create personnel (operators) to assign to receiving and 
implementation records
- Receive PO's with paint color, quantities, and batch numbers
- Log used batches in an implementation ledger
- Automatic merging of batches with the same batch number
- Live updating of on-hand inventory
- Reconciliation and error-checking process to ensure receiving
and implementation ledgers are consistent after a PO receive/edit
session
- Tracking of avialable inventory to ensure a batch is not 
implemented more than it has been received

### History & Metrics
- Garner a variety of historical data including:
  - Specific batches
  - Specific PO's
  - PO's in set timeframes or specific date ranges
- See average vessel usage in a 30-day period from
most recent record in the implementation ledger
- Show all batches connected to dispensing system on a given date

### Storage
- Local SQLite database
- Easy backup of existing database and creation of clean database
- Automatic and manual database backups

### Other
- Contains installer project to build a software installer (removing
previous versions in the process) on other PC's.