//
//  MikiTapticVibrationController.m
//  MikiAPI_iOS_Framework
//
//  Created by 黄建豪 on 2017/9/2.
//  Copyright © 2017年 黄建豪. All rights reserved.
//

#import "CMVibrationController.h"
#import "Utils.h"
#import <AudioToolbox/AudioToolbox.h>
#import <CoreHaptics/CoreHaptics.h>

@implementation CMVibrationController


+(void)VibrationAsLight{
    
    if (@available(iOS 13.0, *)) {
//        CHHapticEngine *engine= [[CHHapticEngine alloc] ];
//        engine.playsHapticsOnly = YES;
    } else {
        if(IS_OS_9_OR_LATER == YES){
            UIImpactFeedbackGenerator *hap = [[UIImpactFeedbackGenerator alloc] init];
            [hap prepare];
            [hap initWithStyle:UIImpactFeedbackStyleLight];
            [hap impactOccurred];
        }
        else{
            AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
        }
    }

    
}
+(void)VibrationAsMedium{
    if(IS_OS_9_OR_LATER == YES){
        UIImpactFeedbackGenerator *hap = [[UIImpactFeedbackGenerator alloc] init];
        [hap prepare];
        [hap initWithStyle:UIImpactFeedbackStyleMedium];
        [hap impactOccurred];
    }
    else{
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
    }
}
+(void)VibrationAsHeavy{
    if(IS_OS_9_OR_LATER == YES){
        UIImpactFeedbackGenerator *hap = [[UIImpactFeedbackGenerator alloc] init];
        [hap prepare];
        [hap initWithStyle:UIImpactFeedbackStyleHeavy];
        [hap impactOccurred];
    }
    else{
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
    }
}

+(void)VibrationAsSoft{
    if(IS_OS_9_OR_LATER == YES){
        UIImpactFeedbackGenerator *hap = [[UIImpactFeedbackGenerator alloc] init];
        [hap prepare];
        if (@available(iOS 13.0, *)) {
            [hap initWithStyle:UIImpactFeedbackStyleSoft];
        } else {
            // Fallback on earlier versions
        }
        [hap impactOccurred];
    }
    else{
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
    }
}

+(void)VibrationAsRigid{
    if(IS_OS_9_OR_LATER == YES){
        UIImpactFeedbackGenerator *hap = [[UIImpactFeedbackGenerator alloc] init];
        [hap prepare];
        if (@available(iOS 13.0, *)) {
            [hap initWithStyle:UIImpactFeedbackStyleRigid];
        } else {
            // Fallback on earlier versions
        }
        [hap impactOccurred];
    }
    else{
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
    }
}
@end
