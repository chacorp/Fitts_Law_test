close all
clear all
clc
%% 가져오기 옵션을 설정하고 데이터 가져오기
opts = delimitedTextImportOptions("NumVariables", 3);

% 범위 및 구분 기호 지정
opts.DataLines = [2, Inf];
opts.Delimiter = ",";

% 열 이름과 유형 지정
opts.VariableNames = ["width", "distance", "meanTCT"];
opts.VariableTypes = ["double", "double", "double"];

% 파일 수준 속성 지정
opts.ExtraColumnsRule = "ignore";
opts.EmptyLineRule = "read";

% 데이터 가져오기
result = readtable("C:\Users\User\Desktop\Dynamics of HCI_HW3_20204571_SihunCha\FittsLaw_taskResult.csv", opts);

%% 임시 변수 지우기
clear opts

% IV1 너비 가져오기
w = result.width';
% IV2 거리 가져오기
d = result.distance';

% y값 = DV 평균 소요시간 가져오기
y = result.meanTCT';

% x값 = index of difficulty
x = d./w;
x = log2(x+1);

% 피팅
plot(x,y,'.'); 
xlabel('index of difficulty');
ylabel('mean trial completion time');

% model fuction
modelfun = @(b,x)b(1)*x(:,1)+b(2)  % b: parameters, x: independent variable

beta0 = [0,0]; % initial parameters, a: 0, b:0
mdl = fitnlm(x, y, modelfun, beta0)

a = mdl.Coefficients.Estimate(1);
b = mdl.Coefficients.Estimate(2);

y_prediction = a*x + b;

hold on;

% 선형 그래프
plot(x,y_prediction); 

final_a = mdl.Coefficients.Estimate(1)

final_b = mdl.Coefficients.Estimate(2)
