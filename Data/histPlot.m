close all; clear; clc;

%% read data
time = csvread('time.csv');
counts = csvread('counts.csv');
velocities = csvread('velocities.csv');
sights = csvread('sights.csv');
sizes = csvread('sizes.csv');

%% Convert 0 to NaN
velocities(velocities == 0) = NaN;
sights(sights == 0) = NaN;
sizes(sizes == 0) = NaN;

%% Velocities histogram

figure()
for i = 1:size(velocities, 1)
   temp1 = velocities(i, :);
   temp2 = temp1(temp1 > 0);
   histogram(temp1)
   pause(0.1)
end

%% Sights
figure()
for i = 1:size(sights, 1)
   temp1 = sights(i, :);
   temp2 = temp1(temp1 > 0);
   histogram(temp2)
   pause(0.1)
end

%% Sizes
figure()
for i = 1:size(sizes, 1)
   temp1 = sizes(i, :);
   temp2 = temp1(temp1 > 0);
   histogram(temp2)
   pause(0.1)
end