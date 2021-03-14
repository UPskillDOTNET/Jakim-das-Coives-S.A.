import React from 'react';
import { NavigationContainer } from '@react-navigation/native';

import UserContextProvider from './src/contexts/UserContext';
import MainStack from './src/stacks/MainStack.js';

export default () => {
    return (
        //means that the app will have information all the time about the user
        <UserContextProvider>
            <NavigationContainer>
                <MainStack />
            </NavigationContainer>
        </UserContextProvider>
    );
}
