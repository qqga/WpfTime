using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace WpfTime
{
    public abstract class BaseVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropChanged([CallerMemberName]string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public static class VmExt
    {
        public static void Bind<TVm, TProp, TObj, TObjProp>(this TVm vm, Expression<Func<TVm, TProp>> prop, TObj obj, Expression<Func<TObj, TObjProp>> propObj, bool setOnBind = true) where TVm : BaseVm
        {
            MemberExpression propMemberVMExpression = prop.Body as MemberExpression;
            MemberExpression propMemberObjExpression = propObj.Body as MemberExpression;
            System.Reflection.PropertyInfo propertyInfoVm = propMemberVMExpression.Member as System.Reflection.PropertyInfo;
            System.Reflection.PropertyInfo propertyInfoObj = propMemberObjExpression.Member as System.Reflection.PropertyInfo;

            string currentPropertySetName = null;

            void SetObjProp(string propName)
            {
                currentPropertySetName = propName;
                object value = propertyInfoVm.GetValue(vm);
                propertyInfoObj.SetValue(obj, value);
                currentPropertySetName = null;
            }

            void SetVmProp(string propName)
            {
                currentPropertySetName = propName;
                object value = propertyInfoObj.GetValue(obj);
                propertyInfoVm.SetValue(vm, value);
                currentPropertySetName = null;
            }

            vm.PropertyChanged += (s, e) =>
            {
                if(e.PropertyName == propMemberVMExpression.Member.Name && string.IsNullOrEmpty(currentPropertySetName))
                    SetObjProp(e.PropertyName);
            };

            if(obj is INotifyPropertyChanged notifyPropertyObj)
            {
                notifyPropertyObj.PropertyChanged += (s, e) =>
                {
                    if(e.PropertyName == propMemberObjExpression.Member.Name && string.IsNullOrEmpty(currentPropertySetName))
                    {
                        SetVmProp(e.PropertyName);
                    }
                };
            }

            if(setOnBind)
                SetVmProp(propertyInfoVm.Name);

        }
    }
}
