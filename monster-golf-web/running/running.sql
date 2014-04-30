CREATE TABLE AW_Runner (
	RunnerId int identity(1,1),
	Name varchar(100),
	Email varchar(100),
	EmailWeekly bit,
	EmailMonthly bit,
	TxtPhone varchar(100),
	TxtWeekly bit,
	TxtMonthly bit,
	ProgressMileage varchar(25),
	Miles float,
	ProgressWorkout varchar(25),
	Workouts float
)
CREATE TABLE AW_RunnerWorkout (
	WorkoutId int identity(1,1),
	RunnerId int,
	WorkoutDate datetime,
	WorkoutType varchar(20),
	WorkoutMiles float,
	WorkoutMinutes int,
	WorkoutSeconds int,
	WorkoutCalories int,
)

select RunnerId,Name,ProgressMileage,Miles,ProgressWorkout,Workouts from AW_Runner
select Count(*) as Workouts, Sum(WorkoutMiles) as WorkoutMiles, Sum(WorkoutMinutes) as WorkoutMinutes, Sum(WorkoutSeconds) as WorkoutSeconds from AW_RunnerWorkout

SELECT WorkoutId, WorkoutDate, WorkoutType, WorkoutMiles, WorkoutMinutes, WorkoutSeconds, WorkoutCalories FROM AW_RunnerWorkout WHERE RunnerId = 
select * from AW_RunnerWorkout
select * from AW_RunnerWorkoutType

delete from AW_RunnerWorkout where workoutid = 5
SELECT WorkoutType, Sum(WorkoutMiles) as WorkoutMiles, Sum(WorkoutMinutes) as WorkoutMinutes, Sum(WorkoutSeconds) as WorkoutSeconds, Sum(WorkoutCalories) as WorkoutCalories
FROM AW_RunnerWorkout WHERE RunnerId = 2
GROUP BY WorkoutType


insert into AW_RunnerWorkoutType values ('Run');
insert into AW_RunnerWorkoutType values ('Walk');
insert into AW_RunnerWorkoutType values ('Bike');
insert into AW_RunnerWorkoutType values ('Crossfit');
insert into AW_RunnerWorkoutType values ('Gym Workout');
insert into AW_RunnerWorkoutType values ('Row');
