/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarStream : MonoBehaviour
{
    public Car firstCar = null;
    public Car lastCar = null;
    public int size = 0;
    public CarStream nextStream;
    public int followDistance = 5;

    /// <summary>
    /// 判断两车的位置关系,以pointer的朝向判断target的位置
    /// </summary>
    /// <returns>pointer在前target在后返回true;pointer在后target在前返回false</returns>
    private bool judgeLocation(Car pointer ,Car target)
    {
        Vector3 dir1 = pointer.transform.forward.normalized;
        Vector3 dir2 = (target.transform.position - pointer.transform.position).normalized;
        if (Vector3.Dot(dir1, dir2) < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 判断车流与car的匹配关系
    /// </summary>
    /// <returns>若car位于车流的firstCar之后且位于lastCar之前，认为car与车流是可匹配的，即car可合流入车流</returns>

    //方法不完备，当车辆可汇入车流做队头或者队尾时，仍然应该认为车辆与车流是匹配的
    public bool isMatched(Car car)
    {
        if(judgeLocation(this.firstCar,car) && !judgeLocation(this.lastCar, car))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void insertCar(Car car)
    {
        if (car == null) return;

        this.size++;

        Car pointer = this.firstCar;
        while (pointer != null)
        {
            //判断车辆插入位置,要考虑到车辆坐标与朝向
            //在该车流中找到第一个在car后面的车辆，并在其之前插入
            if (!judgeLocation(pointer,car))
            {
                if(pointer == this.firstCar)
                {
                    car.behind = pointer;
                    pointer.front = car;
                    this.firstCar = car;
                }
                else
                {
                    car.behind = pointer;
                    car.front = pointer.front;
                    car.front.behind = car;
                    car.behind.front = car;
                }
                return;
            }
        }
        //车流未找到插入位置，在末端插入
        this.lastCar.behind = car;
        car.front = this.lastCar;
        this.lastCar = car;
    }

    public void insertStream(CarStream stream)
    {
        Car pointer = stream.firstCar;
        while(pointer != null)
        {
            this.insertCar(pointer);
        }
    }

    public void removeCar(Car car)
    {
        this.size--;
        if(car == this.firstCar)
        {
            this.firstCar = this.firstCar.behind;
            this.firstCar.front = null;
        }
        else if(car == this.lastCar)
        {
            this.lastCar = this.lastCar.front;
            this.lastCar.behind = null;
        }
        else
        {
            car.front.behind = car.behind;
            car.behind.front = car.front;
            car.front = null;
            car.behind = null;
        }
    }
}
*/