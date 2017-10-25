pointLaborData = read.csv("PointLaborData.csv")

#Remove Outliers
pointLaborData <- pointLaborData[-c(1,2,3,4,5,6,7,9,11,14,16,17,18,20,30,31,37,40),]

#Prepare Data
pointLaborData <- pointLaborData[which(pointLaborData$En_hours > 0),]
pointLaborData <- pointLaborData[which(pointLaborData$Pm_hours > 0),]
pointLaborData <- pointLaborData[which(pointLaborData$Sf_hours > 0),]
pointLaborData <- pointLaborData[which(pointLaborData$Gr_hours > 0),]
pointLaborData <- pointLaborData[which(pointLaborData$Cm_hours > 0),]
pointLaborData$Total_Points <- pointLaborData$IO_Points + pointLaborData$Comm_Points

#Create Models
engModel <- lm(En_hours ~ Total_Points, data = pointLaborData)

#Display Models
summary(engModel)
plot(predict(engModel), engModel$En_hours, xlab = "predicted", ylab = "actual")
abline(a = 0, b = 1)

par(mfrow = c(2, 2))
plot(engModel)



