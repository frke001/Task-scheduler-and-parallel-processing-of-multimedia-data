# Task Scheduler and Parallel Processing of Multimedia Data

This project implements a task scheduler and parallel processing of multimedia data using an arbitrary object-oriented programming language. The project includes a programmatic API for the task scheduler, a logically separated GUI application for task scheduling that uses the API, and a task type for parallel processing of multimedia data.

## Task Scheduler

The task scheduler is implemented as part of a library, with a simple API that allows users to schedule tasks. The API allows users to specify the maximum number of tasks that can be executed concurrently, as well as scheduling algorithms, such as FIFO (FCFS) and priority scheduling. Users can also specify start date and time, total allowed execution time, and deadline for task completion or termination.

In addition to these basic functionalities, the task scheduler also supports priority scheduling with preemption, as well as scheduling based on time slices, where tasks of higher priority receive more time slices for execution. The scheduler also enables working with resources, allowing tasks to lock resources during execution and preventing or detecting and resolving deadlock situations. The problem of priority inversion is solved using the PIP or PCP algorithm.

## Parallel Processing of Multimedia Data

The project includes a task type for multimedia file processing, which allows for the simultaneous processing of a large amount of input files. Users can specify the allowed level of parallelism (e.g., allowed number of utilized CPU cores) for each task, and the task can be scheduled for execution using the task scheduler. The task is designed to perform parallel processing on a single multimedia file, achieving acceleration, and uses a specific algorithm for the course subject.

## GUI Application and Task Persistence

The project includes a non-blocking GUI application for task scheduling, which allows users to specify the scheduling method and all properties of the created task. The GUI application supports the viewing of tasks in progress, with progress bars reporting the progress of the task, as well as the creation, starting, pausing, resuming, stopping, and removing of completed or stopped tasks. Task serialization and deserialization are enabled, with serialized fields including an array of input file paths and a path for generating output files.

## User-Space File System

The project also includes a driver for a user-space file system, which uses the task scheduler API and allows for the processing of multimedia data by tasks. The file system contains an input folder into which multimedia files can be copied, and after the files are copied into the input folder, the processing of those files begins. After the processing of the files is complete, the results are available in the output folder.


## GUI

![image](https://user-images.githubusercontent.com/93668747/231578034-3136af12-1cd6-4d06-93ec-3c747589f481.png)
![image](https://user-images.githubusercontent.com/93668747/231578515-0dd56bbe-5a9a-43bf-8309-df21206abc1c.png)

