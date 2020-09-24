%% Residual plot
figure;
plot(x,y_prediction-y,'.r');
xlabel('x');
ylabel('residual');