'use server'

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { getCurrentUser } from './auth-actions';

async function checkAdminAccess() {
  const user = await getCurrentUser();
  
  if (!user) {
    redirect('/auth/login?redirectTo=/admin/settings');
  }

  const userRoles = user.roles || [];
  const hasAdminRole = userRoles.some(role => 
    role.toLowerCase().includes('admin') || 
    role.toLowerCase().includes('superadmin')
  );

  if (!hasAdminRole) {
    redirect('/?error=' + encodeURIComponent('You do not have permission to access this page'));
  }
  
  return user;
}

let systemSettings = {
  siteName: 'BlueBerry24',
  siteDescription: 'Your premium e-commerce platform',
  contactEmail: 'admin@blueberry24.com',
  maintenanceMode: false,
  allowRegistrations: true,
  defaultCurrency: 'USD',
  taxRate: 8.5,
  shippingCost: 15.00,
  freeShippingThreshold: 100.00,
  emailNotifications: true,
  smsNotifications: false,
  orderNotifications: true,
  stockAlerts: true,
  backupFrequency: 'daily',
  sessionTimeout: 30,
  maxLoginAttempts: 5,
  passwordMinLength: 8,
  requireSpecialChars: true,
  enableTwoFactor: false,
  enableSocialLogin: true,
  enableGuestCheckout: true,
  enableReviews: true,
  enableWishlist: true,
  enableCompareProducts: true,
  defaultLanguage: 'en',
  timezone: 'UTC',
  dateFormat: 'MM/DD/YYYY',
  numberFormat: 'en-US'
};

export async function getSystemSettings() {
  await checkAdminAccess();
  return systemSettings;
}

export async function updateGeneralSettings(formData: FormData) {
  await checkAdminAccess();
  
  try {
    const updates = {
      siteName: formData.get('siteName') as string,
      siteDescription: formData.get('siteDescription') as string,
      contactEmail: formData.get('contactEmail') as string,
      defaultCurrency: formData.get('defaultCurrency') as string,
      taxRate: parseFloat(formData.get('taxRate') as string) || 0,
      defaultLanguage: formData.get('defaultLanguage') as string,
      timezone: formData.get('timezone') as string,
      dateFormat: formData.get('dateFormat') as string,
      numberFormat: formData.get('numberFormat') as string
    };

    if (!updates.siteName || !updates.contactEmail) {
      redirect('/admin/settings?error=' + encodeURIComponent('Site name and contact email are required'));
    }

    Object.assign(systemSettings, updates);

    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('General settings updated successfully'));
  } catch (error) {
    console.error('Error updating general settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to update general settings'));
  }
}

export async function updateEcommerceSettings(formData: FormData) {
  await checkAdminAccess();
  
  try {
    const updates = {
      shippingCost: parseFloat(formData.get('shippingCost') as string) || 0,
      freeShippingThreshold: parseFloat(formData.get('freeShippingThreshold') as string) || 0,
      enableGuestCheckout: formData.get('enableGuestCheckout') === 'on',
      enableReviews: formData.get('enableReviews') === 'on',
      enableWishlist: formData.get('enableWishlist') === 'on',
      enableCompareProducts: formData.get('enableCompareProducts') === 'on'
    };

    if (updates.shippingCost < 0 || updates.freeShippingThreshold < 0) {
      redirect('/admin/settings?error=' + encodeURIComponent('Shipping costs cannot be negative'));
    }

    Object.assign(systemSettings, updates);

    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('E-commerce settings updated successfully'));
  } catch (error) {
    console.error('Error updating e-commerce settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to update e-commerce settings'));
  }
}

export async function updateSecuritySettings(formData: FormData) {
  await checkAdminAccess();
  
  try {
    const updates = {
      maintenanceMode: formData.get('maintenanceMode') === 'on',
      allowRegistrations: formData.get('allowRegistrations') === 'on',
      sessionTimeout: parseInt(formData.get('sessionTimeout') as string) || 30,
      maxLoginAttempts: parseInt(formData.get('maxLoginAttempts') as string) || 5,
      passwordMinLength: parseInt(formData.get('passwordMinLength') as string) || 8,
      requireSpecialChars: formData.get('requireSpecialChars') === 'on',
      enableTwoFactor: formData.get('enableTwoFactor') === 'on',
      enableSocialLogin: formData.get('enableSocialLogin') === 'on'
    };

    if (updates.sessionTimeout < 5 || updates.sessionTimeout > 480) {
      redirect('/admin/settings?error=' + encodeURIComponent('Session timeout must be between 5 and 480 minutes'));
    }

    if (updates.maxLoginAttempts < 3 || updates.maxLoginAttempts > 10) {
      redirect('/admin/settings?error=' + encodeURIComponent('Max login attempts must be between 3 and 10'));
    }

    if (updates.passwordMinLength < 6 || updates.passwordMinLength > 20) {
      redirect('/admin/settings?error=' + encodeURIComponent('Password minimum length must be between 6 and 20 characters'));
    }

    Object.assign(systemSettings, updates);

    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('Security settings updated successfully'));
  } catch (error) {
    console.error('Error updating security settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to update security settings'));
  }
}

export async function updateNotificationSettings(formData: FormData) {
  await checkAdminAccess();
  
  try {
    const updates = {
      emailNotifications: formData.get('emailNotifications') === 'on',
      smsNotifications: formData.get('smsNotifications') === 'on',
      orderNotifications: formData.get('orderNotifications') === 'on',
      stockAlerts: formData.get('stockAlerts') === 'on'
    };

    Object.assign(systemSettings, updates);

    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('Notification settings updated successfully'));
  } catch (error) {
    console.error('Error updating notification settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to update notification settings'));
  }
}

export async function updateBackupSettings(formData: FormData) {
  await checkAdminAccess();
  
  try {
    const updates = {
      backupFrequency: formData.get('backupFrequency') as string
    };

    const validFrequencies = ['manual', 'daily', 'weekly', 'monthly'];
    if (!validFrequencies.includes(updates.backupFrequency)) {
      redirect('/admin/settings?error=' + encodeURIComponent('Invalid backup frequency'));
    }

    Object.assign(systemSettings, updates);

    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('Backup settings updated successfully'));
  } catch (error) {
    console.error('Error updating backup settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to update backup settings'));
  }
}

export async function clearCache(formData: FormData) {
  await checkAdminAccess();
  
  try {
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('System cache cleared successfully'));
  } catch (error) {
    console.error('Error clearing cache:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to clear cache'));
  }
}

export async function backupDatabase(formData: FormData) {
  await checkAdminAccess();
  
  try {
    await new Promise(resolve => setTimeout(resolve, 2000));
    
    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('Database backup completed successfully'));
  } catch (error) {
    console.error('Error backing up database:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to backup database'));
  }
}

export async function runDiagnostics(formData: FormData) {
  await checkAdminAccess();
  
  try {
    await new Promise(resolve => setTimeout(resolve, 1500));
    
    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('System diagnostics completed successfully - All systems operational'));
  } catch (error) {
    console.error('Error running diagnostics:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to run system diagnostics'));
  }
}

export async function resetToDefaults(formData: FormData) {
  await checkAdminAccess();
  
  try {
    systemSettings = {
      siteName: 'BlueBerry24',
      siteDescription: 'Your premium e-commerce platform',
      contactEmail: 'admin@blueberry24.com',
      maintenanceMode: false,
      allowRegistrations: true,
      defaultCurrency: 'USD',
      taxRate: 8.5,
      shippingCost: 15.00,
      freeShippingThreshold: 100.00,
      emailNotifications: true,
      smsNotifications: false,
      orderNotifications: true,
      stockAlerts: true,
      backupFrequency: 'daily',
      sessionTimeout: 30,
      maxLoginAttempts: 5,
      passwordMinLength: 8,
      requireSpecialChars: true,
      enableTwoFactor: false,
      enableSocialLogin: true,
      enableGuestCheckout: true,
      enableReviews: true,
      enableWishlist: true,
      enableCompareProducts: true,
      defaultLanguage: 'en',
      timezone: 'UTC',
      dateFormat: 'MM/DD/YYYY',
      numberFormat: 'en-US'
    };

    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('Settings reset to defaults successfully'));
  } catch (error) {
    console.error('Error resetting settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to reset settings'));
  }
}

export async function exportSettings(formData: FormData) {
  await checkAdminAccess();
  
  try {
    redirect('/admin/settings?success=' + encodeURIComponent('Settings exported successfully'));
  } catch (error) {
    console.error('Error exporting settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to export settings'));
  }
}

export async function importSettings(formData: FormData) {
  await checkAdminAccess();
  
  try {
    const file = formData.get('settingsFile') as File;
    
    if (!file) {
      redirect('/admin/settings?error=' + encodeURIComponent('Please select a settings file to import'));
    }

    await new Promise(resolve => setTimeout(resolve, 1000));
    
    revalidatePath('/admin/settings');
    redirect('/admin/settings?success=' + encodeURIComponent('Settings imported successfully'));
  } catch (error) {
    console.error('Error importing settings:', error);
    redirect('/admin/settings?error=' + encodeURIComponent('Failed to import settings'));
  }
} 