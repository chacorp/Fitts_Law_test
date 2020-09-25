%% Obtain R2

SStot = sum((y-mean(y)).^2);
SSres = sum((y-y_prediction).^2);

R2 = 1-SSres/SStot

%% adjusted R2
% n은 index of difficulty의 갯수
n = length(x);

% p는 IV들의 갯수
p = 2;
R2_adj = 1-(1-R2)*(n-1)/(n-p-1)
