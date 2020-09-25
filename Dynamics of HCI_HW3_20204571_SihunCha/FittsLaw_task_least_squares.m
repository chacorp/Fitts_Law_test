close all
clear all
clc

%% 가져오기 옵션을 설정하고 데이터 가져오기
opts = spreadsheetImportOptions("NumVariables", 3);

% 시트와 범위 지정
opts.Sheet = "FittsLaw_trial_result";
opts.DataRange = "D2:F13";

% 열 이름과 유형 지정
opts.VariableNames = ["w", "d", "MeanTCT"];
opts.VariableTypes = ["double", "double", "double"];

% 데이터 가져오기
result = readtable("/Users/sihuncha/Documents/Dynamics-HCI/Fitts_Law_test/Dynamics of HCI_HW3_20204571_SihunCha/FittsLaw_trial_result.xlsx", opts, "UseExcel", false);

%% 임시 변수 지우기
clear opts

% IV1 너비 가져오기
w = result.w';
% IV2 거리 가져오기
d = result.d';

% y값 = DV 평균 소요시간 가져오기
y = result.MeanTCT';

% x값 = index of difficulty
x = log2(d./w+1);

% 피팅
plot(x, y, '.'); 
xlabel('index of difficulty (bit)');
ylabel('mean trial completion time (seconds)');

% model fuction
modelfun = @(b,x)b(1) * x(:,1)+b(2)  % b: parameters, x: independent variable

beta0 = [0,0]; % initial parameters, a: 0, b:0
mdl = fitnlm(x, y, modelfun, beta0)

a = mdl.Coefficients.Estimate(1);
b = mdl.Coefficients.Estimate(2);

y_prediction = a * x + b;

hold on;

%% 선형 그래프
plot(x, y_prediction,'.'); 

final_a = a

final_b = b
