//
//  MikiAPIController.h
//  MikiiOSNative
//
//  Created by 黄建豪 on 2017/9/3.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface CloudMacacaAPIController : NSObject

typedef void (*AlertDialogCallback)(int callbackType);
typedef void (*DatePickerDialogCallback)(char* date);
typedef void (*RequestCallback)(int status);

//CMVibrationController 的接口
+(void)CM_VibrationAsPeek;
+(void)CM_VibrationAsPop;
+(void)CM_VibrationAsNope;

//CMDialogController 的接口
+(void)CM_ShowRateUsDialog:(NSString*)msg andForCancelText:(NSString*)cancelText andForRateText:(NSString*)rateText andForLaterText:(NSString*) laterText andForAppleID:(NSString*) appId;
+(void) CM_ShowAlertDialogForTitle:(NSString*)title andForMsg:(NSString*)msg andForOkText:(NSString*)okText;

+(void) CM_ShowAlertDialogWithCallbackForTitle:(NSString*)title andForMsg:(NSString*)msg andForPositiveText:(NSString*)positiveText andForNegitiveText: (NSString*)negativeText;
+(void) CM_ShowDatePickerWithCallback:(NSString*)okText andForCancel:(NSString*)cancelText;


+(void) CM_SetAlertDialogCallback :(AlertDialogCallback) cb;
+(void) CM_SetDatePickerDialogCallback :(DatePickerDialogCallback) cb;
+(void)CM_ShowToastMessage:(NSString*)msg;

//CMShareController 的接口
+(void)CM_ShowShareForFilePaths:(NSString*) filePath andForSubject:(NSString*) subject andForMsg:(NSString*)msg andForTypeId:(int) typeId ;

//CMGameCenterController 的接口
+(void)CM_ShowAchievement;
+(void)CM_ShowLeaderboard;
+(void)CM_UploadNewScoreForboardId:(NSString*)boardId andForScore:(int)score;
+(void)CM_UnlockAnAchievement:(NSString*)achievementId;
+(void)CM_InitGameCenter;

+(void)CM_RequestIDFA;
+(void) CM_SetRequestIDFACallback :(RequestCallback) cb;
+(int) CM_GetIDFARequestStatus;


+(void) CM_SendEmail:(NSString*)to Subject:(NSString*)subject Body:(NSString *)body;
@end
