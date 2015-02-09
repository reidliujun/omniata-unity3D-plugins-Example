#import <iOmniataAPI/iOmniataAPI.h>
/**
 * Converts C style string to NSString
 */
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

/**
 * Call initialize with api_key, uid and debug
 */
extern void Initialize(const char* api_key, const char* uid, const char* org)
{
	[iOmniataAPI initializeWithApiKey:GetStringParam(api_key) UserId:GetStringParam(uid) OrgName:GetStringParam(org)];
}

/**
 * Call TrackRevenue with total and currency_code
 */
extern void TrackRevenue(const double total, const char* currency_code)
{
	[iOmniataAPI trackPurchaseEvent:total currency_code: GetStringParam(currency_code)];
}

/**
 * Call TrackEvent with type and parameters
 * Convert parameters to NSMutableDictionary
 */
extern void TrackEvent(const char* type,const char *parameters) {
    NSString *attris = GetStringParam(parameters);
    NSString *decodedString = [attris stringByRemovingPercentEncoding];
    NSArray *attributesArray = [decodedString componentsSeparatedByString:@"\n"];
    NSMutableDictionary *paraDict = [[NSMutableDictionary alloc] init];
    for (int i=0; i < [attributesArray count]; i++) {
        NSString *keyValuePair = [attributesArray objectAtIndex:i];
        NSRange range = [keyValuePair rangeOfString:@"="];
        if (range.location != NSNotFound) {
            NSString *key = [keyValuePair substringToIndex:range.location];
            NSString *value = [keyValuePair substringFromIndex:range.location+1];
            [paraDict setObject:value forKey:key];
        }
    }
    [iOmniataAPI trackEvent: GetStringParam(type):paraDict];
}

/**
 * Call TrackLoad
 */

extern void TrackLoadWithParameters(const char *parameters)
{
    NSString *attris = GetStringParam(parameters);
    NSString *decodedString = [attris stringByRemovingPercentEncoding];
    NSArray *attributesArray = [decodedString componentsSeparatedByString:@"\n"];
    NSMutableDictionary *paraDict = [[NSMutableDictionary alloc] init];
    for (int i=0; i < [attributesArray count]; i++) {
        NSString *keyValuePair = [attributesArray objectAtIndex:i];
        NSRange range = [keyValuePair rangeOfString:@"="];
        if (range.location != NSNotFound) {
            NSString *key = [keyValuePair substringToIndex:range.location];
            NSString *value = [keyValuePair substringFromIndex:range.location+1];
            [paraDict setObject:value forKey:key];
        }
    }
    [iOmniataAPI trackLoadEventWithParameters:paraDict];
}

/**
 * Call TrackRevenue with total, currency_code and additional parameters.
 */
extern void TrackRevenueWithParameters(const double total, const char* currency_code, const char *parameters)
{
    NSLog(@"track revenue in ios plugin");
    NSString *attris = GetStringParam(parameters);
    NSString *decodedString = [attris stringByRemovingPercentEncoding];
    NSArray *attributesArray = [decodedString componentsSeparatedByString:@"\n"];
    NSMutableDictionary *paraDict = [[NSMutableDictionary alloc] init];
    for (int i=0; i < [attributesArray count]; i++) {
        NSString *keyValuePair = [attributesArray objectAtIndex:i];
        NSRange range = [keyValuePair rangeOfString:@"="];
        if (range.location != NSNotFound) {
            NSString *key = [keyValuePair substringToIndex:range.location];
            NSString *value = [keyValuePair substringFromIndex:range.location+1];
            [paraDict setObject:value forKey:key];
        }
    }
    NSLog(@"track revenue in ios plugin before calling trackpurchaseevent");
    [iOmniataAPI trackPurchaseEvent:total currency_code:GetStringParam(currency_code) additional_params:paraDict];
}

/**
 * Call GetChannelMessage with channelID
 * Return NSString result
 */

extern void LoadChannelMessage(const int channelID){
    static NSString * result;
    [iOmniataAPI loadMessagesForChannel:channelID completionHandler:^(OMT_CHANNEL_STATUS cs){
        NSArray* omniValues = [iOmniataAPI getChannelMessages];
        result = [omniValues description];
        NSLog(@"channel message:%@",omniValues);
    }];
}

extern void EnablePushNotifications(const char* device_token){
    [iOmniataAPI enablePushNotifications:GetStringParam(device_token)];
}

extern void DisablePushNotifications(){
    [iOmniataAPI disablePushNotifications];
}

/**
 * Call Log method with message
 * Log in the Xcode console as "Omniata: <message>"
 */
extern void Log(const char* message)
{
    NSLog(@"%@: %@", @"Omniata", GetStringParam(message));
}

/**
 * Call setloglevel with log priority
 */

extern void SetLogLevel(int priority){
    [iOmniataAPI setLogLevel:priority];
}




