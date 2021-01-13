//
//  iCloudKeyValue.mm
//  iCloudKeyValue
//
//  Created by Gennadii Potapov on 03/08/18.
//  Copyright © 2016 General Arcade. All rights reserved.
//

#import <Foundation/Foundation.h>
extern "C"{
    void iCloudKV_Synchronize();
    void iCloudKV_SetString(char * key, char * value) ;
    void iCloudKV_SetInt(char * key, int value);
    void iCloudKV_SetFloat(char * key, float value) ;
    char* iCloudKV_GetString(char * key);
    int iCloudKV_GetInt(char * key);
    float iCloudKV_GetFloat(char * key) ;
    void iCloudKV_Reset();
    bool iCloud_Enable();
}
void iCloudKV_Synchronize() {
    [[NSUbiquitousKeyValueStore defaultStore] synchronize];
}

void iCloudKV_SetString(char * key, char * value) {
    NSString* nsValue =[NSString stringWithUTF8String:value];
    [[NSUbiquitousKeyValueStore defaultStore] setString:nsValue forKey:[NSString stringWithUTF8String:key]];
}

void iCloudKV_SetInt(char * key, int value) {
    [[NSUbiquitousKeyValueStore defaultStore] setObject:[NSNumber numberWithInt:value] forKey:[NSString stringWithUTF8String:key]];
    
}

void iCloudKV_SetFloat(char * key, float value) {
    [[NSUbiquitousKeyValueStore defaultStore] setObject:[NSNumber numberWithFloat:value] forKey:[NSString stringWithUTF8String:key]];
}


char* iCloudKV_GetString(char * key) {
    NSString * string = [[NSUbiquitousKeyValueStore defaultStore] stringForKey:[NSString stringWithUTF8String:key]];
    
    if (string == nil) string = [NSString new];
    const char* cString = string.UTF8String;
    char* _unityString = (char*)malloc(strlen(cString) + 1);
    strcpy(_unityString, cString);
    
    return _unityString;
    //return UnityStringFromNSString();
}


int iCloudKV_GetInt(char * key) {
    NSNumber * num = (NSNumber *)([[NSUbiquitousKeyValueStore defaultStore] objectForKey:[NSString stringWithUTF8String:key]]);
    int i = 0;
    if (num != nil)
        i = [num intValue];
    return i;
}


float iCloudKV_GetFloat(char * key) {
    NSNumber * num = (NSNumber *)([[NSUbiquitousKeyValueStore defaultStore] objectForKey:[NSString stringWithUTF8String:key]]);
    float i = 0;
    if (num != nil)
        i = [num floatValue];
    return i;
}

void iCloudKV_Reset() {
    NSUbiquitousKeyValueStore *kvStore = [NSUbiquitousKeyValueStore defaultStore];
    NSDictionary *kvd = [kvStore dictionaryRepresentation];
    NSArray *arr = [kvd allKeys];
    for (int i=0; i < arr.count; i++){
        NSString *key = [arr objectAtIndex:i];
        [kvStore removeObjectForKey:key];
    }
    [[NSUbiquitousKeyValueStore defaultStore] synchronize];
}

bool iCloud_Enable(){
    NSFileManager *manager = [NSFileManager defaultManager]; 
    // 判断iCloud是否可用 
    // 参数传nil表示使用默认容器 
    NSURL *url = [manager URLForUbiquityContainerIdentifier:nil]; 
    // 如果URL不为nil, 则表示可用 
    if (url != nil) { 
        return true; 
    }
    NSLog(@"iCloud 不可用"); 
    return false;
}

/*
 char* UnityStringFromNSString(NSString* string)
 {
 if (string == nil) string = [NSString new];
 const char* cString = string.UTF8String;
 char* _unityString = (char*)malloc(strlen(cString) + 1);
 strcpy(_unityString, cString);
 return _unityString;
 }*/
