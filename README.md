# RevitSyncAlarm
A simple nondisruptive but very noticeable sync alarm warning Add-in for revit.

![image](https://user-images.githubusercontent.com/8847598/38228875-22ccc66c-3749-11e8-911b-fb4eb44d3f73.png)

After 20 minutes without sync.
![image](https://user-images.githubusercontent.com/8847598/38228889-2cc6d7ac-3749-11e8-8b24-46787fe3a5e3.png)

After 40 minutes without sync.
![image](https://user-images.githubusercontent.com/8847598/38228901-3db63e36-3749-11e8-89f6-437468559203.png)


## Limitations ("Bugs?")

* The add-in only checks if a file is workshared. So if you detach form central and preserve all the worksets it will still give you the warnings.
* The add-in is session based and doesn't track individual files. If you are working in more than one workshared file it will do whatever your last action was (Open a worshared file, not Sync for 20 and 40min, and Synchronize) independently from the file you are in.
* For some reason the close event is not resetting the ribbon color.
