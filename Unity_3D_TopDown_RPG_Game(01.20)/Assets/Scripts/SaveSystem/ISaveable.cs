using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
	//저장할 때 저장할 데이터를 파싱해서 반환한다.
	object CaptureState();

	//로드할 때 저장된 데이터를 가져와서 파싱한다.
	void RestoreState(object state);
}
