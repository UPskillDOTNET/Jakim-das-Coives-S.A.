import React from 'react';
import {createBottomTabNavigator} from '@react-navigation/bottom-tabs';

import CustomTabBar from '../components/CustomTabBar';

import Search from '../screens/Search';
import Reservas from '../screens/Reservas';
import Profile from '../screens/Profile';

const Tab = createBottomTabNavigator();

export default () => (
    <Tab.Navigator tabBar= {props=><CustomTabBar {...props} />}>
        <Tab.Screen name="Search" component={Search} />
        <Tab.Screen name="Reservas" component={Reservas} />
        <Tab.Screen name="Profile" component={Profile} />
    </Tab.Navigator>
);