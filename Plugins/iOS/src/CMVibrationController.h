//
//  MikiTapticVibrationController.h
//  MikiAPI_iOS_Framework
//
//  Created by 黄建豪 on 2017/9/2.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface CMVibrationController : NSObject
+(void)VibrationAsLight;
+(void)VibrationAsMedium;
+(void)VibrationAsHeavy;
+(void)VibrationAsSoft;
+(void)VibrationAsRigid;

@end
